using AutoMapper;
using EventEdu.Application.DTOs;
using EventEdu.Application.DTOs.Category;
using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace EventEdu.Persistence.Services
{
	public class SpeakerService : ISpeakerService
	{
		private readonly AppDbContext _context;
		private readonly IEventSpeakerReadRepository _eventSpeakerReadRepository;
		private readonly IEventSpeakerWriteRepository _eventSpeakerWriteRepository;
		private readonly ISpeakerWriteRepository _speakerWriteRepository;
		private readonly ISpeakerReadRepository _speakerReadRepository;
		private readonly ISpeakerDetailWriteRepository _speakerDetailWriteRepository;
		private readonly ISpeakerDetailReadRepository _speakerDetailReadRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly IMapper _mapper;
		private readonly IValidator<CreateSpeakerDTO> _createSpeakerValidator;
		private readonly IValidator<UpdateSpeakerDTO> _updateSpeakerValidator;
		
		readonly private IHttpContextAccessor _contextAccessor;
		public SpeakerService(AppDbContext context,
			ISpeakerWriteRepository speakerWriteRepository,
			ISpeakerReadRepository speakerReadRepository,
			ISpeakerDetailWriteRepository speakerDetailWriteRepository, 
			ISpeakerDetailReadRepository speakerDetailReadRepository,
			ILanguageReadRepository languageReadRepository, IMapper mapper,
			IValidator<CreateSpeakerDTO> createSpeakerValidator, 
			IValidator<UpdateSpeakerDTO> updateSpeakerValidator
			, IHttpContextAccessor contextAccessor,
			IEventSpeakerReadRepository eventSpeakerReadRepository,
			IEventSpeakerWriteRepository eventSpeakerWriteRepository
			)
		{
			_context = context;
			_speakerWriteRepository = speakerWriteRepository;
			_speakerReadRepository = speakerReadRepository;
			_speakerDetailWriteRepository = speakerDetailWriteRepository;
			_speakerDetailReadRepository = speakerDetailReadRepository;
			_languageReadRepository = languageReadRepository;
			_mapper = mapper;
			_createSpeakerValidator = createSpeakerValidator;
			_updateSpeakerValidator = updateSpeakerValidator;
			_contextAccessor = contextAccessor;
			_eventSpeakerReadRepository = eventSpeakerReadRepository;
			_eventSpeakerWriteRepository = eventSpeakerWriteRepository;
			
		}

		public async Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO)
		{

			//var language = await _context.Languages.FirstOrDefaultAsync(x => x.Id == createSpeakerDTO.LanguageId);
			//if (language == null)
			//{
			//	throw new Exception("Selected language not found");
			//}

			var validationResult = await _createSpeakerValidator.ValidateAsync(createSpeakerDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			var language = await _languageReadRepository.GetByIdAsync(createSpeakerDTO.LanguageId.ToString());
			if (language == null)
			{
				throw new NotFoundException("Selected language not found");
			}

			//bool isSpeakerExist = await _context.SpeakerDetails.AnyAsync(x => x.FullName == createSpeakerDTO.FullName && x.LanguageId == createSpeakerDTO.LanguageId);
			//if (isSpeakerExist)
			//{
			//	throw new Exception("This speaker already exists for the selected language.");
			//}

			//var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speaker.Id, createSpeakerDTO.LanguageId);
			//if (isSpeakerExist != null)
			//{
			//	throw new Exception("This speaker already exists for the selected language.");
			//}

			//var speaker = new Speaker
			//{
			//	Id = Guid.NewGuid(),
			//	CreatedDate = DateTime.UtcNow,
			//	UpdatedDate = DateTime.UtcNow,
			//	ImageUrl = createSpeakerDTO.ImageUrl,
			//	Email = createSpeakerDTO.Email,
			//	FacebookLink = createSpeakerDTO.FacebookLink,
			//	TwitterLink = createSpeakerDTO.TwitterLink,
			//	InstagramLink = createSpeakerDTO.InstagramLink
			//};
			var speaker = _mapper.Map<Speaker>(createSpeakerDTO);
			speaker.Id = Guid.NewGuid();
			speaker.CreatedDate = DateTime.UtcNow;
			speaker.UpdatedDate = DateTime.UtcNow;

			var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speaker.Id, createSpeakerDTO.LanguageId);
			if (isSpeakerExist != null)
			{
				throw new BadRequestException("This speaker already exists for the selected language.");
			}

			//_context.Speakers.Add(speaker);
			//await _context.SaveChangesAsync();

			await _speakerWriteRepository.AddAsync(speaker);
			await _speakerWriteRepository.SaveChangeAsync();

			//var speakerDetail = new SpeakerDetail
			//{
			//	Id = Guid.NewGuid(),
			//	FullName = createSpeakerDTO.FullName,
			//	Bio = createSpeakerDTO.Bio,
			//	SpeakerId = speaker.Id,
			//	LanguageId = createSpeakerDTO.LanguageId,
			//	CreatedDate = DateTime.UtcNow,
			//	UpdatedDate = DateTime.UtcNow,
			//};

			var speakerDetail = _mapper.Map<SpeakerDetail>(createSpeakerDTO);
			speakerDetail.Id = Guid.NewGuid();
			speakerDetail.SpeakerId = speaker.Id;
			speakerDetail.CreatedDate = DateTime.UtcNow;
			speakerDetail.UpdatedDate = DateTime.UtcNow;

			//_context.SpeakerDetails.Add(speakerDetail);
			//await _context.SaveChangesAsync();
			await _speakerDetailWriteRepository.AddAsync(speakerDetail);
			await _speakerDetailWriteRepository.SaveChangeAsync();
		}

		public async Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode)
		{
			//var language = await _context.Languages.FirstOrDefaultAsync(x => x.IsoCode == isoCode);
			//if (language == null)
			//{
			//	language = await _context.Languages.FirstAsync();
			//}

			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				throw new NotFoundException("Language not found");
			}
			var speakers = await _speakerReadRepository.GetSpeakersByLanguageAsync(language.Id);
			var speakerDetails = await _speakerDetailReadRepository.GetByLanguageIdAsync(language.Id);
			var result = speakers.Select(s => new GetSpeakerDTO
			{
				Id = s.Id,
				FullName = speakerDetails.FirstOrDefault(sd => sd.SpeakerId == s.Id)?.FullName,
				Bio = speakerDetails.FirstOrDefault(sd => sd.SpeakerId == s.Id)?.Bio,
				IsoCode = language.IsoCode,
				ImagePath = s.ImageUrl,
				Email = s.Email,
				FacebookLink = s.FacebookLink,
				TwitterLink = s.TwitterLink,
				InstagramLink = s.InstagramLink
			}).ToList();
			return result;

			//var speakers = await _context.Speakers
			// .Where(s => _context.SpeakerDetails
			//  .Any(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id))
			// .Select(s => new GetSpeakerDTO
			// {
			//  Id = s.Id,
			//  FullName = _context.SpeakerDetails
			//   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
			//   .Select(sd => sd.FullName)
			//   .FirstOrDefault(),
			//  Bio = _context.SpeakerDetails
			//   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
			//   .Select(sd => sd.Bio)
			//   .FirstOrDefault(),
			//  IsoCode = language.IsoCode,
			//  ImagePath = s.ImageUrl,
			//  Email = s.Email,
			//  FacebookLink = s.FacebookLink,
			//  TwitterLink = s.TwitterLink,
			//  InstagramLink = s.InstagramLink
			// })
			// .ToListAsync();
			//return speakers;
		}

		//	var speakers = await _speakerReadRepository.GetSpeakersByLanguageAsync(language.Id);

		//	var speakerDTOs = new List<GetSpeakerDTO>();
		//	foreach (var speaker in speakers)
		//	{
		//		var speakerDetail = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speaker.Id, language.Id);
		//		if (speakerDetail != null)
		//		{
		//			speakerDTOs.Add(new GetSpeakerDTO
		//			{
		//				Id = speaker.Id,
		//				FullName = speakerDetail.FullName,
		//				Bio = speakerDetail.Bio,
		//				IsoCode = language.IsoCode,
		//				ImagePath = speaker.ImageUrl,
		//				Email = speaker.Email,
		//				FacebookLink = speaker.FacebookLink,
		//				TwitterLink = speaker.TwitterLink,
		//				InstagramLink = speaker.InstagramLink
		//			});
		//		}
		//	}

		//	return speakerDTOs;
		//}

		public async Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO)
		{
			var validationResult = await _updateSpeakerValidator.ValidateAsync(updateSpeakerDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}
			//var speakerDetail = await _context.SpeakerDetails.FirstOrDefaultAsync(x => x.Id == speakerId);
			var speakerDetail = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speakerId, updateSpeakerDTO.LanguageId);
			if (speakerDetail == null)
			{
				throw new NotFoundException("Speaker not found.");
			}

			//	bool isSpeakerExist = await _context.SpeakerDetails
			//.AnyAsync(x => x.FullName == updateSpeakerDTO.FullName &&
			//			   x.LanguageId == updateSpeakerDTO.LanguageId &&
			//			   x.Id != speakerId);

			//	if (isSpeakerExist)
			//	{
			//		throw new Exception("This speaker name already exists for the selected language.");
			//	}


			//var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speakerId, updateSpeakerDTO.LanguageId);
			//if (isSpeakerExist != null && isSpeakerExist.Id != speakerId)
			//{
			//	throw new BadRequestException("This speaker name already exists for the selected language.");
			//}

			////
			var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAsync(speakerId);
			if (isSpeakerExist == null)
			{
				throw new NotFoundException("Speaker not found.");
			}


			//	bool isSpeakerExist = await _speakerDetailReadRepository.Table
			//.AnyAsync(x => x.FullName == updateSpeakerDTO.FullName &&
			//			   x.LanguageId == updateSpeakerDTO.LanguageId &&
			//			   x.SpeakerId != speakerDetail.SpeakerId); // **DÜZƏLİŞ BURADADIR!**

			//	if (isSpeakerExist)
			//	{
			//		throw new Exception("This speaker name already exists for the selected language.");
			//	}

			//speakerDetail.FullName = updateSpeakerDTO.FullName;
			//speakerDetail.Bio = updateSpeakerDTO.Bio;
			//speakerDetail.LanguageId = updateSpeakerDTO.LanguageId;
			//speakerDetail.UpdatedDate = DateTime.UtcNow;

			_mapper.Map(updateSpeakerDTO, speakerDetail);
			speakerDetail.UpdatedDate = DateTime.UtcNow;

			//var speaker = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == speakerDetail.SpeakerId);
			var speaker = await _speakerReadRepository.GetByIdAsync(speakerId.ToString());
			if (speaker != null)
			{
				//speaker.ImageUrl = updateSpeakerDTO.ImageUrl;
				//speaker.Email = updateSpeakerDTO.Email;
				//speaker.FacebookLink = updateSpeakerDTO.FacebookLink;
				//speaker.TwitterLink = updateSpeakerDTO.TwitterLink;
				//speaker.InstagramLink = updateSpeakerDTO.InstagramLink;
				//speaker.UpdatedDate = DateTime.UtcNow;

				_mapper.Map(updateSpeakerDTO, speaker);
				speaker.UpdatedDate = DateTime.UtcNow;
			}
			//await _context.SaveChangesAsync();
			await _speakerWriteRepository.SaveChangeAsync();
		}

		public async Task SoftDeleteSpeakerAsync(Guid speakerId)
		{
			var speaker = await _speakerReadRepository.GetByIdAsync(speakerId.ToString());
			if (speaker == null)
			{
				throw new NotFoundException("Speaker not found");
			}

			speaker.SoftDelete();
			_speakerWriteRepository.Update(speaker);

			var speakerDetails = await _speakerDetailReadRepository.GetAll()
				.Where(x => x.SpeakerId == speakerId).ToListAsync();

			foreach (var detail in speakerDetails)
			{
				detail.SoftDelete();
				_speakerDetailWriteRepository.Update(detail);
			}
			await _speakerWriteRepository.SaveChangeAsync();
		}

		public async Task RestoreSpeakerAsync(Guid speakerId)
		{
			var speaker = await _speakerReadRepository.GetByIdAsync(speakerId.ToString());
			if (speaker == null)
			{
				throw new NotFoundException("Speaker not found");
			}
			speaker.Restore();
			_speakerWriteRepository.Update(speaker);

			var speakerDetails = await _speakerDetailReadRepository.GetAll()
				.Where(x => x.SpeakerId == speakerId).ToListAsync();

			foreach (var detail in speakerDetails)
			{
				detail.Restore();
				_speakerDetailWriteRepository.Update(detail);
			}
			await _speakerWriteRepository.SaveChangeAsync();
		}

		public async Task<(List<Speaker>, List<SpeakerDetail>)> GetSpeakersAllAsync()
		{
			var language = _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault();
			var isoCode = language.Split(",").FirstOrDefault();
			var languages = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			var speakerDetails = await _speakerDetailReadRepository.GetAll().Where(a => a.LanguageId == languages.Id).ToListAsync();
			var speak = await _speakerReadRepository.GetAll()
				.Include(a => a.EventSpeakers)
				.Include(a => a.SpeakerDetails)

				.ToListAsync();



			return (speak, speakerDetails);
		}

		public async Task<SpeakDetailsVM> GetSpeakersByIdAsync(string id)
		{
			var speaker = await _speakerReadRepository.GetByIdAsync(id);

			// speaker tapılmadıqda boş dictionary qaytarırıq

			var speakerDetail = await _speakerDetailReadRepository
				.GetAll()
				.FirstOrDefaultAsync(a => a.SpeakerId == speaker.Id);

			var spVm = new SpeakDetailsVM()
			{
				Speaker = speaker,
				SpeakerDetail = speakerDetail
			};
			return spVm;

		}

		public async Task<List<SpeakerDetail>> GetSpeakersEventByIdAsync(string eventId)
		{
			var language =  _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault();
			var isoCode = language.Split(",").FirstOrDefault();	
			var languages= await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			var eventSpeaker = await _eventSpeakerReadRepository.GetAll().FirstOrDefaultAsync(a => a.EventId == Guid.Parse(eventId));

			var speaker = await _speakerDetailReadRepository.GetAll().Where(a => a.SpeakerId == eventSpeaker.SpeakerId && a.LanguageId == languages.Id).ToListAsync();

			return speaker;

		}
	}
}
