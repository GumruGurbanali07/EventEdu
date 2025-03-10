using AutoMapper;
using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
	public class SpeakerService : ISpeakerService
	{

		private readonly AppDbContext _context;
		private readonly ISpeakerWriteRepository _speakerWriteRepository;
		private readonly ISpeakerReadRepository _speakerReadRepository;
		private readonly ISpeakerDetailWriteRepository _speakerDetailWriteRepository;
		private readonly ISpeakerDetailReadRepository _speakerDetailReadRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly IMapper _mapper;
		private readonly IValidator<CreateSpeakerDTO> _createSpeakerValidator;
		private readonly IValidator<UpdateSpeakerDTO> _updateSpeakerValidator;
		public SpeakerService(AppDbContext context, ISpeakerWriteRepository speakerWriteRepository, ISpeakerReadRepository speakerReadRepository, ISpeakerDetailWriteRepository speakerDetailWriteRepository, ISpeakerDetailReadRepository speakerDetailReadRepository, ILanguageReadRepository languageReadRepository, IMapper mapper, IValidator<CreateSpeakerDTO> createSpeakerValidator, IValidator<UpdateSpeakerDTO> updateSpeakerValidator)
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
		}

		public async Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO)
		{
			
			//var language = await _context.Languages.FirstOrDefaultAsync(x => x.Id == createSpeakerDTO.LanguageId);
			//if (language == null)
			//{
			//	throw new Exception("Selected language not found");
			//}
			var language = await _languageReadRepository.GetByIdAsync(createSpeakerDTO.LanguageId.ToString());
			if (language == null)
			{
				throw new Exception("Selected language not found");
			}

			//bool isSpeakerExist = await _context.SpeakerDetails.AnyAsync(x => x.FullName == createSpeakerDTO.FullName && x.LanguageId == createSpeakerDTO.LanguageId);
			//if (isSpeakerExist)
			//{
			//	throw new Exception("This speaker already exists for the selected language.");
			//}
			var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(createSpeakerDTO.SpeakerId, createSpeakerDTO.LanguageId);
			if (isSpeakerExist != null)
			{
				throw new Exception("This speaker already exists for the selected language.");
			}

			var speaker = new Speaker
			{
				Id = Guid.NewGuid(),
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.UtcNow,
				ImageUrl = createSpeakerDTO.ImageUrl,
				Email = createSpeakerDTO.Email,
				FacebookLink = createSpeakerDTO.FacebookLink,
				TwitterLink = createSpeakerDTO.TwitterLink,
				InstagramLink = createSpeakerDTO.InstagramLink
			};

			//_context.Speakers.Add(speaker);
			//await _context.SaveChangesAsync();

			await _speakerWriteRepository.AddAsync(speaker);
			await _speakerWriteRepository.SaveChangeAsync();

			var speakerDetail = new SpeakerDetail
			{
				Id = Guid.NewGuid(),
				FullName = createSpeakerDTO.FullName,
				Bio = createSpeakerDTO.Bio,
				SpeakerId = speaker.Id,
				LanguageId = createSpeakerDTO.LanguageId,
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.UtcNow,
			};

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
				throw new Exception("Language not found");
			}

			//	var speakers = await _context.Speakers
			//  .Where(s => _context.SpeakerDetails
			//   .Any(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id))
			//  .Select(s => new GetSpeakerDTO
			//  {
			//   Id = s.Id,
			//   FullName = _context.SpeakerDetails
			//	   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
			//	   .Select(sd => sd.FullName)
			//	   .FirstOrDefault(),
			//   Bio = _context.SpeakerDetails
			//	   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
			//	   .Select(sd => sd.Bio)
			//	   .FirstOrDefault(),
			//   IsoCode = language.IsoCode,
			//   ImagePath = s.ImageUrl,
			//   Email = s.Email,
			//   FacebookLink = s.FacebookLink,
			//   TwitterLink = s.TwitterLink,
			//   InstagramLink = s.InstagramLink
			//  })
			//  .ToListAsync();

			//	return speakers;

			//}
			var speakers = await _speakerReadRepository.GetSpeakersByLanguageAsync(language.Id);

			var speakerDTOs = new List<GetSpeakerDTO>();
			foreach (var speaker in speakers)
			{
				var speakerDetail = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speaker.Id, language.Id);
				if (speakerDetail != null)
				{
					speakerDTOs.Add(new GetSpeakerDTO
					{
						Id = speaker.Id,
						FullName = speakerDetail.FullName,
						Bio = speakerDetail.Bio,
						IsoCode = language.IsoCode,
						ImagePath = speaker.ImageUrl,
						Email = speaker.Email,
						FacebookLink = speaker.FacebookLink,
						TwitterLink = speaker.TwitterLink,
						InstagramLink = speaker.InstagramLink
					});
				}
			}

			return speakerDTOs;
		}

		public async Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO)
		{
			//var speakerDetail = await _context.SpeakerDetails.FirstOrDefaultAsync(x => x.Id == speakerId);
			var speakerDetail = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speakerId, updateSpeakerDTO.LanguageId);
			if (speakerDetail == null)
			{
				throw new Exception("Speaker not found.");
			}

			//	bool isSpeakerExist = await _context.SpeakerDetails
			//.AnyAsync(x => x.FullName == updateSpeakerDTO.FullName &&
			//			   x.LanguageId == updateSpeakerDTO.LanguageId &&
			//			   x.Id != speakerId);

			//	if (isSpeakerExist)
			//	{
			//		throw new Exception("This speaker name already exists for the selected language.");
			//	}

			var isSpeakerExist = await _speakerDetailReadRepository.GetBySpeakerIdAndLanguageIdAsync(speakerId, updateSpeakerDTO.LanguageId);
			if (isSpeakerExist != null && isSpeakerExist.Id != speakerId)
			{
				throw new Exception("This speaker name already exists for the selected language.");

			}


			speakerDetail.FullName = updateSpeakerDTO.FullName;
			speakerDetail.Bio = updateSpeakerDTO.Bio;
			speakerDetail.LanguageId = updateSpeakerDTO.LanguageId;
			speakerDetail.UpdatedDate = DateTime.UtcNow;

			//var speaker = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == speakerDetail.SpeakerId);
			var speaker = await _speakerReadRepository.GetByIdAsync(speakerId.ToString());
			if (speaker != null)
			{
				speaker.ImageUrl = updateSpeakerDTO.ImageUrl;
				speaker.Email = updateSpeakerDTO.Email;
				speaker.FacebookLink = updateSpeakerDTO.FacebookLink;
				speaker.TwitterLink = updateSpeakerDTO.TwitterLink;
				speaker.InstagramLink = updateSpeakerDTO.InstagramLink;
				speaker.UpdatedDate = DateTime.UtcNow;
			}
			//await _context.SaveChangesAsync();
			await _speakerWriteRepository.SaveChangeAsync();
		}

		public async Task SoftDeleteSpeakerAsnyc(Guid speakerId)
		{
			var speakers = await _context.Speakers
				.Include(x=>x.SpeakerDetails)
				.FirstOrDefaultAsync(x => x.Id == speakerId);
			if (speakers == null)
			{
				throw new Exception("Speaker not found");
			}
			speakers.SoftDelete();
			foreach(var detail in speakers.SpeakerDetails)
			{
				detail.SoftDelete();
			}			
			await _context.SaveChangesAsync();

		}
		public async Task RestoreSpeakerAsync(Guid speakerId)
		{
			var speakers = await _context.Speakers
				.Include(x=>x.SpeakerDetails)
				.FirstOrDefaultAsync(x => x.Id == speakerId);

			if (speakers == null)
			{
				throw new Exception("Category not found");
			}

			speakers.Restore();

			foreach (var detail in speakers.SpeakerDetails)
			{
				detail.Restore();
			}

			await _context.SaveChangesAsync();

		}




	}
}
