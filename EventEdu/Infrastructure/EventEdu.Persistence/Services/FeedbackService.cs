using System;
using System.Threading.Tasks;
using EventEdu.Application.DTOs.Feedback;
using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Application.Services
{
	public class FeedbackService :Hub ,  IFeedbackService
	{
		private readonly IFeedbackWriteRepository _feedbackWriteRepository;
		private readonly IFeedBackDetailWriteRepository _feedBackDetailWriteRepository;
		private readonly IFeedBackDetailReadRepository _feedBackDetailReadRepository;
		private readonly ISubsEventReadRepository _subsEventReadRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly IFeedbackReadRepository _feedbackReadRepository;

		public FeedbackService(
			IFeedbackWriteRepository feedbackWriteRepository,
			IFeedBackDetailWriteRepository feedBackDetailWriteRepository,
			ISubsEventReadRepository subsEventReadRepository,
			IFeedBackDetailReadRepository feedBackDetailReadRepository,
			IFeedbackReadRepository feedbackReadRepository,
			ILanguageReadRepository languageReadRepository)
		{
			_feedbackWriteRepository = feedbackWriteRepository;
			_feedBackDetailWriteRepository = feedBackDetailWriteRepository;
			_subsEventReadRepository = subsEventReadRepository;
			_feedBackDetailReadRepository = feedBackDetailReadRepository;
			_feedbackReadRepository = feedbackReadRepository;
			_languageReadRepository = languageReadRepository;
		}

		public async Task<bool> AddFeedBackWithLanguage(AddFeedBackDTO addFeedBackDTO, Guid subscriptionId)
		{
			var subsEvent = await _subsEventReadRepository
				.GetAll()
				.FirstOrDefaultAsync(s => s.EventId == addFeedBackDTO.EventId && s.SubscriptionId == subscriptionId);

			if (subsEvent == null)
				return false; 

			var feadback = await _feedbackReadRepository.GetAll().Where(a=>a.Id==subsEvent.EventId).ToListAsync();

			double rating = feadback.Any() ? feadback.Average(f => (int)f.RatingEvenets) : 0;
			var feedback = new FeedBack
			{
				Id = Guid.NewGuid(),
				Rating = rating,
				EventId = addFeedBackDTO.EventId,
				SubscriptionId = subscriptionId, 
				CreatedDate = DateTime.UtcNow
			};

			await _feedbackWriteRepository.AddAsync(feedback);
			await _feedbackWriteRepository.SaveChangeAsync();

			
			var feedbackDetail = new FeedBackDetail
			{
				Id = Guid.NewGuid(),
				FeedBackId = feedback.Id,
				LanguageId = addFeedBackDTO.LanguageId,
				Comment = addFeedBackDTO.Comment,
				CreatedDate = DateTime.UtcNow
			};

			await _feedBackDetailWriteRepository.AddAsync(feedbackDetail);
			await _feedBackDetailWriteRepository.SaveChangeAsync();

			return true; 
		}

		public Task<GetFeedbackDTO> GetFeedbackAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<List<GetFeedbackDTO>> GetFeedbacksByEventAndLanguageAsync(Guid eventId, string isoCode)
		{
			// 1. ISO koduna əsasən dili tap
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				language = await _languageReadRepository.GetAll().FirstOrDefaultAsync(); // Default language
			}

			// 2. Bütün FeedbackDetail-ları yığ
			var feedbackDetailsQuery = _feedBackDetailReadRepository.GetAll();

			// 3. Event və dilə uyğun Feedback-ləri gətir
			var feedbacks = await _feedbackReadRepository.GetAll()
				.Where(f => f.EventId == eventId &&
							feedbackDetailsQuery.Any(fd => fd.FeedBackId == f.Id && fd.LanguageId == language.Id))
				.Select(f => new GetFeedbackDTO
				{
					Id = f.Id,
					LanguageId = language.Id,
					Rating = f.Rating,
					Comment = feedbackDetailsQuery
						.Where(fd => fd.FeedBackId == f.Id && fd.LanguageId == language.Id)
						.Select(fd => fd.Comment)
						.FirstOrDefault()
				})
				.ToListAsync();
			return feedbacks;
		}



	}
}
