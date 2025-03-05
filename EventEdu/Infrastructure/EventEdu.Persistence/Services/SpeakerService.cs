using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
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

		public SpeakerService(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddSpeakerWithLanguageAsync(CreateSpeakerDTO createSpeakerDTO)
		{
			bool isSpeakerExist = await _context.SpeakerDetails.AnyAsync(x => x.FullName == createSpeakerDTO.FullName && x.LanguageId == createSpeakerDTO.LanguageId);
			if (isSpeakerExist)
			{
				throw new Exception("This speaker already exists for the selected language.");
			}
			var language = await _context.Languages.FirstOrDefaultAsync(x => x.Id == createSpeakerDTO.LanguageId);
			if (language == null)
			{
				throw new Exception("Selected language not found.");
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
			_context.Speakers.Add(speaker);
			await _context.SaveChangesAsync();

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

			_context.SpeakerDetails.Add(speakerDetail);
			await _context.SaveChangesAsync();
		}

		public async Task<List<GetSpeakerDTO>> GetSpeakersByLanguageAsync(string isoCode)
		{
			var language = await _context.Languages.FirstOrDefaultAsync(x => x.IsoCode == isoCode);
			if (language == null)
			{
				language = await _context.Languages.FirstAsync();
			}
			var speakers = await _context.Speakers
	   .Where(s => _context.SpeakerDetails
		   .Any(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id))
	   .Select(s => new GetSpeakerDTO
	   {
		   Id = s.Id,
		   FullName = _context.SpeakerDetails
			   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
			   .Select(sd => sd.FullName)
			   .FirstOrDefault(),
		   Bio = _context.SpeakerDetails
			   .Where(sd => sd.SpeakerId == s.Id && sd.LanguageId == language.Id)
			   .Select(sd => sd.Bio)
			   .FirstOrDefault(),
		   IsoCode = language.IsoCode,
		   ImagePath = s.ImageUrl,
		   Email = s.Email,
		   FacebookLink = s.FacebookLink,
		   TwitterLink = s.TwitterLink,
		   InstagramLink = s.InstagramLink
	   })
	   .ToListAsync();

			return speakers;

		}


		public async Task UpdateSpeakerAsync(Guid speakerId, UpdateSpeakerDTO updateSpeakerDTO)
		{
			var speakerDetail = await _context.SpeakerDetails.FirstOrDefaultAsync(x => x.Id == speakerId);
			if (speakerDetail == null)
			{
				throw new Exception("Speaker not found.");
			}

			bool isSpeakerExist = await _context.SpeakerDetails
		.AnyAsync(x => x.FullName == updateSpeakerDTO.FullName &&
					   x.LanguageId == updateSpeakerDTO.LanguageId &&
					   x.Id != speakerId);

			if (isSpeakerExist)
			{
				throw new Exception("This speaker name already exists for the selected language.");
			}
			speakerDetail.FullName = updateSpeakerDTO.FullName;
			speakerDetail.Bio = updateSpeakerDTO.Bio;
			speakerDetail.LanguageId = updateSpeakerDTO.LanguageId;
			speakerDetail.UpdatedDate = DateTime.UtcNow;

			var speaker = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == speakerDetail.SpeakerId);
			if (speaker != null)
			{
				speaker.ImageUrl = updateSpeakerDTO.ImageUrl;
				speaker.Email = updateSpeakerDTO.Email;
				speaker.FacebookLink = updateSpeakerDTO.FacebookLink;
				speaker.TwitterLink = updateSpeakerDTO.TwitterLink;
				speaker.InstagramLink = updateSpeakerDTO.InstagramLink;
				speaker.UpdatedDate = DateTime.UtcNow;
			}
			await _context.SaveChangesAsync();
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
