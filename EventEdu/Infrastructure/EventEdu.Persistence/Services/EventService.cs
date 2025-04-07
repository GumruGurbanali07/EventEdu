using AutoMapper;
using EventEdu.Application.DTOs.Event;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Repositor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;



namespace EventEdu.Persistence.Services
{
	public class EventService : IEventService
	{
		private readonly IEventReadRepository _eventReadRepository;
		private readonly IEventSpeakerReadRepository _eventSpeakerReadRepository;
		private readonly IEventSpeakerWriteRepository _eventSpeakerWriteRepository;
		private readonly IEventSponsorReadRepository _eventSponsorReadRepository;
		private readonly IEventSponsorWriteRepository _eventSponsorWriteRepository;
		private readonly IEventWriteRepository _eventWriteRepository;
		private readonly ICategoryReadRepository _categoryReadRepository;
		private readonly ICategoryWriteRepository _categoryWriteRepository;
		private readonly ICategoryDetailReadRepository _categoryDetailReadRepository;
		private readonly IEventDetailReadRepository _eventDetailReadRepository;
		private readonly IEventDetailWriteRepository _eventDetailWriteRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		private readonly IValidator<CreateEventDTO> _createValidator;
		private readonly IValidator<UpdateEventDTO> _updateValidator;
		private readonly IMapper _mapper;
		private readonly AppDbContext _context;
		private readonly ISpeakerDetailReadRepository _speakerDetailReadRepository;
		private readonly ISponsorDetailReadRepository _sponsorDetailReadRepository;
		private readonly ISponsorReadRepository _sponsorReadRepository;
		private readonly IFileService _fileService;

		public EventService(IEventReadRepository eventReadRepository, IEventWriteRepository eventWriteRepository, IEventDetailReadRepository eventDetailReadRepository, IEventDetailWriteRepository eventDetailWriteRepository, ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IValidator<CreateEventDTO> createValidator, IValidator<UpdateEventDTO> updateValidator, IMapper mapper, AppDbContext context, IFileService fileService, ICategoryReadRepository categoryReadRepository, ICategoryDetailReadRepository categoryDetailReadRepository, ICategoryWriteRepository categoryWriteRepository, IEventSpeakerWriteRepository eventSpeakerWriteRepository = null, IEventSpeakerReadRepository eventSpeakerReadRepository = null, IEventSponsorReadRepository eventSponsorReadRepository = null, IEventSponsorWriteRepository eventSponsorWriteRepository = null, ISpeakerDetailReadRepository speakerDetailReadRepository = null, ISponsorDetailReadRepository sponsorDetailReadRepository = null, ISponsorReadRepository sponsorReadRepository = null)
		{
			_eventReadRepository = eventReadRepository;
			_eventWriteRepository = eventWriteRepository;
			_eventDetailReadRepository = eventDetailReadRepository;
			_eventDetailWriteRepository = eventDetailWriteRepository;
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
			_createValidator = createValidator;
			_updateValidator = updateValidator;
			_mapper = mapper;
			_context = context;
			_fileService = fileService;
			_categoryReadRepository = categoryReadRepository;
			_categoryDetailReadRepository = categoryDetailReadRepository;
			_categoryWriteRepository = categoryWriteRepository;
			_eventSpeakerWriteRepository = eventSpeakerWriteRepository;
			_eventSpeakerReadRepository = eventSpeakerReadRepository;
			_eventSponsorReadRepository = eventSponsorReadRepository;
			_eventSponsorWriteRepository = eventSponsorWriteRepository;
			_speakerDetailReadRepository = speakerDetailReadRepository;
			_sponsorDetailReadRepository = sponsorDetailReadRepository;
			_sponsorReadRepository = sponsorReadRepository;
		}

		public async Task AddEventWithLanguageAsync(CreateEventDTO createEventDTO)
		{
			// Language mövcudluğunu yoxlayaq
			var language = await _languageReadRepository.GetByIdAsync(createEventDTO.LanguageId.ToString());
			if (language == null)
			{
				throw new NotFoundException("Language not found");
			}

			var categoryDetails = await _categoryDetailReadRepository.GetByIdAsync(createEventDTO.CategoriesId.ToString());
			var newFile = await _fileService.UploadAsync(createEventDTO.FormFile);
			// Yeni Event yaradılır
			var eventEntity = new Event()
			{
				StartDate = createEventDTO.StartDate,
				EndDate = createEventDTO.EndDate,
				CategoryId = categoryDetails.CategoryId, // CategoryId düzgün ötürülməli
				ImageUrl = newFile
			};

			try
			{
				// Event əlavə edilir
				await _eventWriteRepository.AddAsync(eventEntity);
				await _eventWriteRepository.SaveChangeAsync();

				// EventDetail yaradılır
				var eventDetail = new EventDetail
				{
					EventId = eventEntity.Id,
					Title = createEventDTO.Title,
					Description = createEventDTO.Description,
					LanguageId = createEventDTO.LanguageId // LanguageId düzgün ötürülməli
				};

				await _eventDetailWriteRepository.AddAsync(eventDetail);
				await _eventDetailWriteRepository.SaveChangeAsync();




				var sponsorEvents = createEventDTO.EventSponsor.Select(a => new EventSponsor()
				{
					Id = Guid.NewGuid(),
					SponsorId = _sponsorDetailReadRepository.GetAll().FirstOrDefault(sp => sp.Id == a).SponsorId,
					EventId = eventEntity.Id,

				}).ToList();
				await _eventSponsorWriteRepository.AddRangeAsync(sponsorEvents);
				await _eventSponsorWriteRepository.SaveChangeAsync();
				var speakerEvents = createEventDTO.EventSpeaker.Select(se => new EventSpeaker()
				{
					Id = Guid.NewGuid(),
					SpeakerId = _speakerDetailReadRepository.GetAll().FirstOrDefault(st => st.Id == se).SpeakerId,
					EventId = eventEntity.Id,
				}).ToList();

				await _eventSpeakerWriteRepository.AddRangeAsync(speakerEvents);
				await _eventSpeakerWriteRepository.SaveChangeAsync();

				// EventSponsor əlaqələri yaradılır




			}
			catch (Exception ex)
			{
				// Hər hansı bir səhv olarsa, tətbiqdə idarə edilməsi lazım olan səhv
				throw new Exception("Error occurred while creating event: " + ex.Message);
			}
		}


		public async Task<List<GetEventDTO>> GetEventsByLanguageAsync(string isoCode)
		{
			// 1. ISO koduna əsasən dili tap
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				language = await _languageReadRepository.GetAll().FirstOrDefaultAsync(); // Default language
			}

			var eventDetailsQuery = _eventDetailReadRepository.GetAll();

			var events = await _eventReadRepository.GetAll()
				.Where(e => eventDetailsQuery.Any(ed => ed.EventId == e.Id && ed.LanguageId == language.Id))
				.Select(e => new GetEventDTO
				{
					Id = e.Id,
					Title = eventDetailsQuery
						.Where(ed => ed.EventId == e.Id && ed.LanguageId == language.Id)
						.Select(ed => ed.Title)
						.FirstOrDefault(),
					Description = eventDetailsQuery
						.Where(ed => ed.EventId == e.Id && ed.LanguageId == language.Id)
						.Select(ed => ed.Description)
						.FirstOrDefault(),
					StartDate = e.StartDate,
					EndDate = e.EndDate,
					CategoryId = e.CategoryId,
					LanguageId = language.Id,
					IsoCode = language.IsoCode,
					//ImageUrl = e.ImageUrl
				})
				.ToListAsync();

			return events;
		}
		public async Task<GetEventDTO?> GetEventByIdAndLanguageAsync(Guid eventId, string isoCode)
		{
			// Fetch the language by isoCode, if not found, fall back to the first available language.
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				language = await _languageReadRepository.GetAll().FirstOrDefaultAsync();
			}

			var eventDetailsQuery = _eventDetailReadRepository.GetAll();
			// Query for event details in the specified language

			// Fetch the event details based on eventId and the language
			var eventData = await _eventReadRepository.GetAll()
				.Where(e => e.Id == eventId)
				.Select(e => new GetEventDTO
				{
					Id = e.Id,
					Title = eventDetailsQuery
						.Where(ed => ed.EventId == eventId && ed.LanguageId == language.Id)
						.Select(ed => ed.Title)
						.FirstOrDefault(),
					Description = eventDetailsQuery
						.Where(ed => ed.EventId == eventId && ed.LanguageId == language.Id)
						.Select(ed => ed.Description)
						.FirstOrDefault(),
					IsoCode = language.IsoCode,
					ImageUrl = e.ImageUrl,
					CategoryName = e.Category.CategoryDetail
						.Where(cd => cd.LanguageId == language.Id)
						.Select(cd => cd.CategoryName)
						.FirstOrDefault(),
					StartDate = e.StartDate,
					EndDate = e.EndDate,
					LanguageId = language.Id,
					CategoryId = e.CategoryId
				})
				.FirstOrDefaultAsync();

			// If no event is found, throw an exception
			if (eventData == null)
			{
				throw new NotFoundException("Event not found.");
			}

			return eventData;
		}
		public async Task UpdateEventAsync(Guid eventId, UpdateEventDTO dto)
		{
			// 1. Event detalları və əsas entity-ni al
			var eventEntity = await _eventReadRepository.GetByIdAsync(eventId.ToString());
			var eventDetails = await _eventDetailReadRepository
				.GetAll().FirstOrDefaultAsync
				(a => a.EventId == eventEntity.Id);



			var category = await _categoryDetailReadRepository.GetByIdAsync(dto.CategoryId.ToString());


			// 2. Entity-nin əsas sahələrini yenilə
			eventEntity.CategoryId = category.CategoryId;
			eventEntity.StartDate = dto.StartDate;
			eventEntity.EndDate = dto.EndDate;

			if (dto.FormFile != null)
			{
				_fileService.Delete(eventEntity.ImageUrl);
				var newImageUrl = await _fileService.UploadAsync(dto.FormFile);
				eventEntity.ImageUrl = newImageUrl;
			}

			_eventWriteRepository.Update(eventEntity);
			await _eventWriteRepository.SaveChangeAsync();

			eventDetails.Title = dto.Title;
			eventDetails.Description = dto.Description;
			eventDetails.LanguageId = dto.LanguageId;

			_eventDetailWriteRepository.Update(eventDetails);
			await _eventDetailWriteRepository.SaveChangeAsync();


			if (dto.SponsorId != null && dto.SponsorId.Any())
			{
				var eventSP = await _eventSponsorReadRepository.GetAll().Where(a => a.EventId == eventEntity.Id).ToListAsync();

				_eventSponsorWriteRepository.RemoveRange(eventSP);
				await _eventSponsorWriteRepository.SaveChangeAsync();
				var sponsorEntities = dto.SponsorId.Select(id =>
				{
					var sponsor = _sponsorDetailReadRepository.GetAll().FirstOrDefault(s => s.SponsorId == id);
					if (sponsor == null) throw new Exception("Sponsor not found.");

					return new EventSponsor
					{
						Id = Guid.NewGuid(),
						EventId = eventEntity.Id,
						SponsorId = sponsor.SponsorId
					};
				}).ToList();

				await _eventSponsorWriteRepository.AddRangeAsync(sponsorEntities);
				await _eventSponsorWriteRepository.SaveChangeAsync();
			}


			if (dto.SpeakerId != null && dto.SpeakerId.Any())
			{
				var eventSK = await _eventSpeakerReadRepository.GetAll().Where(a=>a.EventId== eventEntity.Id).ToListAsync();
				 _eventSpeakerWriteRepository.RemoveRange(eventSK);
				await _eventSpeakerWriteRepository.SaveChangeAsync();
				var speakerEntities = dto.SpeakerId.Select(id =>
				{
					var speaker = _speakerDetailReadRepository.GetAll().FirstOrDefault(s => s.SpeakerId == id);
					if (speaker == null) throw new Exception("Speaker not found.");

					return new EventSpeaker
					{
						Id = Guid.NewGuid(),
						EventId = eventEntity.Id,
						SpeakerId = speaker.SpeakerId
					};
				}).ToList();

				await _eventSpeakerWriteRepository.AddRangeAsync(speakerEntities);
				await _eventSpeakerWriteRepository.SaveChangeAsync();
			}
		}

		public async Task SoftDeleteEventAsync(Guid eventId)
		{
			var eventEntity = await _eventReadRepository.GetByIdAsync(eventId.ToString());
			if (eventEntity == null)
			{
				throw new NotFoundException("Event not found.");
			}
			eventEntity.SoftDelete();  // Mark the event as deleted
			_eventWriteRepository.Update(eventEntity);

			// Get associated event details
			var eventDetails = await _eventDetailReadRepository.GetAll()
				.Where(x => x.EventId == eventId)
				.ToListAsync();

			// Soft delete each event detail
			foreach (var detail in eventDetails)
			{
				detail.SoftDelete();
				_eventDetailWriteRepository.Update(detail);
			}

			await _eventWriteRepository.SaveChangeAsync();  // Save changes to the database
		}
		public async Task RestoreEventAsync(Guid eventId)
		{
			var eventEntity = await _eventReadRepository.GetByIdAsync(eventId.ToString());
			if (eventEntity == null)
			{
				throw new NotFoundException("Event not found.");
			}
			eventEntity.Restore();  // Restore the event
			_eventWriteRepository.Update(eventEntity);

			// Get associated event details
			var eventDetails = await _eventDetailReadRepository.GetAll()
				.Where(x => x.EventId == eventId)
				.ToListAsync();

			// Restore each event detail
			foreach (var detail in eventDetails)
			{
				detail.Restore();
				_eventDetailWriteRepository.Update(detail);
			}

			await _eventWriteRepository.SaveChangeAsync();  // Save changes to the database
		}

		public async Task<List<EventDetail>> GetEventDetailsAll()
		=> await _eventDetailReadRepository.GetAll().Include(a => a.Language).ToListAsync();

		public async Task<List<Event>> GetEventAll()
		{
			var events = await _eventReadRepository.GetAll()
												   .Include(a => a.Category)
												   .Include(a => a.EventDetails)
												   .Include(a => a.SubsEvents)
												   .Include(a => a.EventSpeakers)
												   .Include(a => a.EventSponsors)

													.ToListAsync();

			return events;
		}

		public async Task<Event> GetEventById(string id)
		{
			if (!Guid.TryParse(id, out var guid)) throw new BadRequestException("Invalid id format");
			var events = await _eventReadRepository.GetByIdAsync(guid.ToString());

			return events;

		}

		public async Task<List<EventDetail>> GetEventDetailAll()
		=> await _eventDetailReadRepository.GetAll().ToListAsync();

		public async Task<EventDetail> GetEventDetailsById(string id)
		{


			var events = await _eventDetailReadRepository.GetByIdAsync(id);

			return events;
		}

		public async Task<List<Event>> GetEventCategoryAsync(string categoryName, string isoCode)
		{
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language != null)
			{
				var category = await _categoryDetailReadRepository.GetAll().FirstOrDefaultAsync(a => a.CategoryName == categoryName && a.LanguageId == language.Id);
				var events = await _eventReadRepository.GetAll().Include(a => a.EventDetails).Where(a => a.CategoryId == category.Id).Select(a => new Event()
				{
					Id = a.Id,
					CategoryId = a.CategoryId,
					EndDate = a.EndDate,
					ImageUrl = a.ImageUrl,
					StartDate = a.StartDate,
					EventDetails = a.EventDetails.Where(ed => ed.LanguageId == language.Id && ed.EventId == a.Id).ToList(),
					EventSpeakers = a.EventSpeakers.Where(ed => ed.EventId == a.Id).ToList(),
				}).ToListAsync();
				return events;
			}
			return null;
		}

		public async Task<(List<Guid>, List<Guid>)> GetEventSpeakerById(string eventId)
		{
			var eventSpeak = await _eventSpeakerReadRepository.GetAll(
				).Where(a => a.EventId == Guid.Parse(eventId) && a.IsDeleted==false).Select(a => a.SpeakerId).ToListAsync();
			var eventSponsor = await _eventSponsorReadRepository.GetAll()
				.Where(a => a.EventId == Guid.Parse(eventId) && a.IsDeleted == false).Select(a => a.SponsorId).ToListAsync();

			return (eventSpeak, eventSponsor);
		}
	}
}