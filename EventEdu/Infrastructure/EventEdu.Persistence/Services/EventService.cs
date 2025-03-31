using AutoMapper;
using EventEdu.Application.DTOs.Event;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ValidationException = FluentValidation.ValidationException;



namespace EventEdu.Persistence.Services
{
	public class EventService : IEventService
	{
		private readonly IEventReadRepository _eventReadRepository;
		private readonly IEventWriteRepository _eventWriteRepository;
		private readonly IEventDetailReadRepository _eventDetailReadRepository;
		private readonly IEventDetailWriteRepository _eventDetailWriteRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		private readonly IValidator<CreateEventDTO> _createValidator;
		private readonly IValidator<UpdateEventDTO> _updateValidator;
		private readonly IMapper _mapper;
		private readonly AppDbContext _context;

		public EventService(IEventReadRepository eventReadRepository, IEventWriteRepository eventWriteRepository, IEventDetailReadRepository eventDetailReadRepository, IEventDetailWriteRepository eventDetailWriteRepository, ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IValidator<CreateEventDTO> createValidator, IValidator<UpdateEventDTO> updateValidator, IMapper mapper, AppDbContext context)
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
		}

		public async Task AddEventWithLanguageAsync(CreateEventDTO createEventDTO)
		{
			var validationResult = await _createValidator.ValidateAsync(createEventDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}
            var language = await _languageReadRepository.GetByIdAsync(createEventDTO.LanguageId);
			if (language == null)
			{
				throw new NotFoundException("Language not found");
			}
			var eventEntity = _mapper.Map<Event>(createEventDTO);
			eventEntity.Id = Guid.NewGuid();
			eventEntity.CreatedDate = DateTime.UtcNow;
			eventEntity.UpdatedDate = DateTime.UtcNow;

			await _eventWriteRepository.AddAsync(eventEntity);
			await _eventWriteRepository.SaveChangeAsync();

			var eventDetail = new EventDetail
			{
				Id = Guid.NewGuid(),
				EventId = eventEntity.Id,
				Title = createEventDTO.Title,
				Description = createEventDTO.Description,
				LanguageId = createEventDTO.LanguageId,
			};
			await _eventDetailWriteRepository.AddAsync(eventDetail);
			await _eventDetailWriteRepository.SaveChangeAsync();			
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

			// Query for event details in the specified language
			var eventDetailsQuery = _eventDetailReadRepository.GetAll();

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
                    //ImageUrl = e.ImageUrl,
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
		public async Task UpdateEventAsync(Guid eventId, UpdateEventDTO updateEventDTO)
		{
			// Validate the updateEventDTO using the relevant validator
			var validationResult = await _updateValidator.ValidateAsync(updateEventDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			// Fetch the event to be updated
			var eventEntity = await _eventReadRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Id == eventId);
			if (eventEntity == null)
			{
				throw new NotFoundException("Event not found.");
			}

			// Check if the event with the same title, language, and category already exists (excluding the current event)
			bool isEventExist = await _context.EventDetails
				.AnyAsync(x => x.Title == updateEventDTO.Title && x.LanguageId == updateEventDTO.LanguageId
				&& x.Event.CategoryId == updateEventDTO.CategoryId && x.EventId != eventId);

			if (isEventExist)
			{
				throw new BadRequestException("This event title already exists for the selected language and category.");
			}

			// Map the properties from the DTO to the existing event entity
			_mapper.Map(updateEventDTO, eventEntity);
			eventEntity.UpdatedDate = DateTime.UtcNow;

			// Update the event entity in the repository
			_eventWriteRepository.Update(eventEntity);
			await _eventWriteRepository.SaveChangeAsync();
		}
		public async Task SoftDeleteEventAsync(Guid eventId)
		{
            var eventEntity = await _eventReadRepository.GetByIdAsync(eventId);
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
            var eventEntity = await _eventReadRepository.GetByIdAsync(eventId);
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



	}

}
