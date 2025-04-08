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
		
		private readonly IEventReadRepository _eventReadRepository;
		private readonly ISubsEventWriteRepository _subsEventWriteRepository;
		private readonly ISubsEventReadRepository _subsEventReadRepository;
		private readonly ISubscriptionReadRepository _subscriptionReadRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly IFeedbackReadRepository _feedbackReadRepository;

		public FeedbackService(
			IFeedbackWriteRepository feedbackWriteRepository,

			ISubsEventReadRepository subsEventReadRepository,
			ISubscriptionReadRepository subscriptionReadRepository,
			IFeedbackReadRepository feedbackReadRepository,
			ILanguageReadRepository languageReadRepository,
			IEventReadRepository eventReadRepository,
			ISubsEventWriteRepository subsEventWriteRepository)
		{
			_feedbackWriteRepository = feedbackWriteRepository;

			_subsEventReadRepository = subsEventReadRepository;
			_subscriptionReadRepository = subscriptionReadRepository;
			_feedbackReadRepository = feedbackReadRepository;

			_eventReadRepository = eventReadRepository;
			_subsEventWriteRepository = subsEventWriteRepository;
		}

		public async Task<bool> AddFeedBackWithLanguage(AddFeedBackDTO addFeedBackDTO)
		{
			// Tədbirin mövcudluğunu yoxlayırıq
			var @event = await _eventReadRepository.GetByIdAsync(addFeedBackDTO.EventId.ToString());
			if (@event == null)
				return false;

			// Yalnız həmin Event-ə aid olan feedback-ləri gətir
			var eventFeedbacks = await _feedbackReadRepository
				.GetAll()
				.Where(f => f.EventId == addFeedBackDTO.EventId)
				.ToListAsync();

		

			var feedback = new FeedBack
			{
				Id = Guid.NewGuid(),
				FullName = "ad", // Əgər istifadəçinin adı varsa, onu da DTO-ya əlavə etmək olar
			
				Rating = addFeedBackDTO.Rating,
				Comment = addFeedBackDTO.Comment,
				EventId = @event.Id,
			};

			await _feedbackWriteRepository.AddAsync(feedback);
			await _feedbackWriteRepository.SaveChangeAsync();

			return true;
		}


		public async Task<List<GetFeedbackDTO>> GetFeedbackAsync()
		{
			var feeadback = await _feedbackReadRepository.GetAll().Select(a=> new GetFeedbackDTO()
			{
				Id=a.Id,
				Rating=a.Rating,
				Comment=a.Comment,
				FullName=a.FullName,
				
			}).ToListAsync();

			return feeadback;
		}

		public async Task<List<GetFeedbackDTO>> GetFeedbacksByEventAndLanguageAsync(Guid eventId)
		{
			// Əgər dil filtrinə ehtiyac varsa, ISO kodu ilə tapmaq üçün buraya əlavə etmək olar

			// SubEvent varsa, onu al (lazımlıdırsa istifadə et)
			var subsevent = await _subsEventReadRepository
				.GetAll()
				.FirstOrDefaultAsync(a => a.EventId == eventId && !a.IsDeleted);

			// Yalnız həmin Event-ə aid olan feedback-ləri gətir
			var eventFeedbacks = await _feedbackReadRepository
		.GetAll()
		.Where(f => f.EventId == eventId)
		.ToListAsync();

			int totalRatings = eventFeedbacks.Sum(f => (int)f.Rating);
			int feedbackCount = eventFeedbacks.Count;

			double averageRating = feedbackCount > 0
				? (double)totalRatings / feedbackCount
				: 0;

			// Event ID-ə görə aid olan Feedback-ləri al
			var feedbacks = await _feedbackReadRepository.GetAll()
				.Where(f => f.EventId == eventId)
				.Select(f => new GetFeedbackDTO
				{
					Id = f.Id,
					FullName = f.FullName,
					Comment = f.Comment,
					TotalRating = averageRating,
					Rating = f.Rating 
					
					// TotalRating əvəzinə bura Rating yazmaq daha məntiqlidir
				})
				.ToListAsync();

			return feedbacks;
		}



	}
}
