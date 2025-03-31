using EventEdu.Application.DTOs.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IFeedbackService
    {
        Task<GetFeedbackDTO> GetFeedbackAsync();    

        Task<bool> AddFeedBackWithLanguage(AddFeedBackDTO addFeedBackDTO, Guid subscriptionId);
        Task<List<GetFeedbackDTO>> GetFeedbacksByEventAndLanguageAsync(Guid eventId, string isoCode);

	}
}
