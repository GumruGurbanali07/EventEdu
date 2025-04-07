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
using System.Reflection.Metadata;
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
		private readonly IFileService _fileService;
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
, IFileService fileService)
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
			_fileService = fileService;
		}

		public async Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO)
		{



			var language = await _languageReadRepository.GetByIdAsync(createSpeakerDTO.LanguageId.ToString());



			var newFile = await _fileService.UploadAsync(createSpeakerDTO.FormFile);
			var speaker = _mapper.Map<Speaker>(createSpeakerDTO);
			speaker.ImageUrl = newFile;
			speaker.CreatedDate = DateTime.UtcNow;
			speaker.UpdatedDate = DateTime.UtcNow;

			var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speaker.Id, createSpeakerDTO.LanguageId);
			if (isSpeakerExist != null)
			{
				throw new BadRequestException("This speaker already exists for the selected language.");
			}



			await _speakerWriteRepository.AddAsync(speaker);
			await _speakerWriteRepository.SaveChangeAsync();



			var speakerDetail = _mapper.Map<SpeakerDetail>(createSpeakerDTO);

			speakerDetail.SpeakerId = speaker.Id;
			speakerDetail.CreatedDate = DateTime.UtcNow;
			speakerDetail.UpdatedDate = DateTime.UtcNow;


			await _speakerDetailWriteRepository.AddAsync(speakerDetail);
			await _speakerDetailWriteRepository.SaveChangeAsync();
		}

		//public async Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode)
		//{
		//	//var language = await _context.Languages.FirstOrDefaultAsync(x => x.IsoCode == isoCode);
		//	//if (language == null)
		//	//{
		//	//	language = await _context.Languages.FirstAsync();
		//	//}

		//	var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
		//	if (language == null)
		//	{
		//		throw new NotFoundException("Language not found");
		//	}
		//	var speakers = await _speakerReadRepository.GetSpeakersByLanguageAsync(language.Id);
		//	var speakerDetails = await _speakerDetailReadRepository.GetByLanguageIdAsync(language.Id);
		//	var result = speakers.Select(s => new GetSpeakerDTO
		//	{
		//		Id = s.Id,
		//		FullName = speakerDetails.FirstOrDefault(sd => sd.SpeakerId == s.Id)?.FullName,
		//		Bio = speakerDetails.FirstOrDefault(sd => sd.SpeakerId == s.Id)?.Bio,
		//		IsoCode = language.IsoCode,
		//		ImagePath = s.ImageUrl,
		//		Email = s.Email,
		//		FacebookLink = s.FacebookLink,
		//		TwitterLink = s.TwitterLink,
		//		InstagramLink = s.InstagramLink
		//	}).ToList();
		//	return result;

		//	//var speakers = await _context.Speakers
		//	// .Where(s => _context.SpeakerDetails
		//	//  .Any(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id))
		//	// .Select(s => new GetSpeakerDTO
		//	// {
		//	//  Id = s.Id,
		//	//  FullName = _context.SpeakerDetails
		//	//   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
		//	//   .Select(sd => sd.FullName)
		//	//   .FirstOrDefault(),
		//	//  Bio = _context.SpeakerDetails
		//	//   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
		//	//   .Select(sd => sd.Bio)
		//	//   .FirstOrDefault(),
		//	//  IsoCode = language.IsoCode,
		//	//  ImagePath = s.ImageUrl,
		//	//  Email = s.Email,
		//	//  FacebookLink = s.FacebookLink,
		//	//  TwitterLink = s.TwitterLink,
		//	//  InstagramLink = s.InstagramLink
		//	// })
		//	// .ToListAsync();
		//	//return speakers;
		//}




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

		public async Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode)
		{
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				throw new NotFoundException("Language not found");
			}

			var speakers = await _speakerReadRepository.GetSpeakersByLanguageAsync(language.Id);
			var speakerDetails = await _speakerDetailReadRepository.GetByLanguageIdAsync(language.Id);

			var result = new List<GetSpeakerDTO>();

			foreach (var speaker in speakers)
			{
				var detail = speakerDetails.FirstOrDefault(sd => sd.SpeakerId == speaker.Id);

				var dto = new GetSpeakerDTO
				{
					Id = speaker.Id,
					FullName = detail?.FullName,
					Bio = detail?.Bio,
					IsoCode = language.IsoCode,
					ImagePath = speaker.ImageUrl,
					Email = speaker.Email,
					FacebookLink = speaker.FacebookLink,
					TwitterLink = speaker.TwitterLink,
					InstagramLink = speaker.InstagramLink
				};

				result.Add(dto);
			}

			return result;
		}


		public async Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO)
		{
			var validationResult = await _updateSpeakerValidator.ValidateAsync(updateSpeakerDTO);
			if (!validationResult.IsValid)
			{
				// Validasiya uğursuz olarsa, istisna atmaq lazımdır
				throw new ValidationException("Validation failed");
			}

			// SpeakerDetail məlumatını tapırıq
			var speakerDetail = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speakerId, updateSpeakerDTO.LanguageId);
			if (speakerDetail == null)
			{
				throw new NotFoundException("SpeakerDetail not found.");
			}

			// Speaker məlumatını tapırıq
			var speaker = await _speakerReadRepository.GetByIdAsync(speakerDetail.SpeakerId.ToString());
			if (speaker == null)
			{
				throw new NotFoundException("Speaker not found.");
			}

			// Yeni şəkil yükləndiyi halda
			if (updateSpeakerDTO.FormFile != null)
			{
				// Əgər köhnə şəkil varsa, onu silirik
				if (!string.IsNullOrEmpty(speaker.ImageUrl))
				{
					_fileService.Delete(speaker.ImageUrl);
				}

				// Yeni şəkil yükləyirik
				var newFile = await _fileService.UploadAsync(updateSpeakerDTO.FormFile);
				speaker.ImageUrl = newFile;
				await _speakerWriteRepository.SaveChangeAsync();
			}

			// Speaker məlumatını yeniləyirik

			speaker.Id = speakerDetail.SpeakerId;
			speaker.FacebookLink = updateSpeakerDTO.FacebookLink;
			speaker.InstagramLink = updateSpeakerDTO.InstagramLink;
			speaker.TwitterLink = updateSpeakerDTO.TwitterLink;
			speaker.Email = updateSpeakerDTO.Email;

			_speakerWriteRepository.Update(speaker);
			await _speakerWriteRepository.SaveChangeAsync();
			// SpeakerDetail məlumatını yeniləyirik
			_mapper.Map(updateSpeakerDTO, speakerDetail);
			_speakerDetailWriteRepository.Update(speakerDetail);
			await _speakerDetailWriteRepository.SaveChangeAsync();


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
			var language = _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault().Split(",").FirstOrDefault();
			

			var languageEntity = await _languageReadRepository.GetByIsoCodeAsync(language);

			// Bütün speaker-ləri detalları ilə birlikdə al
			var speakers = await _speakerReadRepository.GetAll()
			  .Include(a => a.EventSpeakers)
			  .Include(a => a.SpeakerDetails)
			  .ToListAsync();


			var speakerIds = speakers.Select(s => s.Id).ToList();

			var speakerDetails = await _speakerDetailReadRepository.GetAll()
			  .Where(a => a.LanguageId == languageEntity.Id && speakerIds.Contains(a.SpeakerId))
			  .ToListAsync();

			var distinctDetails = speakerDetails
			  .GroupBy(s => s.SpeakerId)
			  .Select(g => g.FirstOrDefault())
			  .ToList();

			return (speakers, distinctDetails);
		}

		public async Task<SpeakDetailsVM> GetSpeakersByIdAsync(string id)
		{
			var speaker = await _speakerReadRepository.GetByIdAsync(id);

			// speaker tapılmadıqda boş dictionary qaytarırıq

			var speakerDetail = await _speakerDetailReadRepository
				.GetAll()
				.FirstOrDefaultAsync(a => a.SpeakerId == speaker.Id && !a.IsDeleted);

			var spVm = new SpeakDetailsVM()
			{
				Speaker = speaker,
				SpeakerDetail = speakerDetail
			};
			return spVm;

		}

		public async Task<List<SpeakerDetail>> GetSpeakersEventByIdAsync(string eventId)
		{
			var language = _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault().Split(",").FirstOrDefault(); ;

			var languages = await _languageReadRepository.GetByIsoCodeAsync(language);
			var eventSpeaker = await _eventSpeakerReadRepository.GetAll().FirstOrDefaultAsync(a => a.EventId == Guid.Parse(eventId));

			var speaker = await _speakerDetailReadRepository.GetAll().Where(a => a.SpeakerId == eventSpeaker.SpeakerId && a.LanguageId == languages.Id && !a.IsDeleted).ToListAsync();

			return speaker;

		}

		public async Task<List<SpeakerDetail>> GetSpeakersAsync()
		=> await _speakerDetailReadRepository.GetAll().ToListAsync();

		public async Task<(List<Speaker>, List<SpeakerDetail>)> GetSpeakersDByIdAsync(string eventId)
		{
			var language = _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault().Split(",").FirstOrDefault(); ;
			var languages = await _languageReadRepository.GetByIsoCodeAsync(language);
			var eventSpeakers = await _eventSpeakerReadRepository.GetAll().Where(a => a.EventId.ToString() == eventId).ToListAsync();

			var speakerIds = eventSpeakers.Select(s => s.SpeakerId).ToList();
			var speakers = await _speakerReadRepository.GetAll().Where(a => speakerIds.Contains(a.Id))
		  .Include(a => a.EventSpeakers)
		  .Include(a => a.SpeakerDetails)
		  .ToListAsync();

			var speakerDetails = await _speakerDetailReadRepository.GetAll()
			  .Where(a => a.LanguageId == languages.Id && speakerIds.Contains(a.SpeakerId))
			  .ToListAsync();

			var distinctDetails = speakerDetails
		  .GroupBy(s => s.SpeakerId)
		  .Select(g => g.FirstOrDefault())
		  .ToList();
			return (speakers, distinctDetails);
		}
	}
}
