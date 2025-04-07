using EventEdu.Application.DTOs.Feedback;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.Controllers
{
	
	public class FeedbackController : Controller
	{

		readonly  private IFeedbackService _feedbackService;

		[HttpGet]
		public async Task<IActionResult> Index()
		{
		    var feedBack = await _feedbackService.GetFeedbackAsync();
			return Ok(feedBack);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid subscriptionId, AddFeedBackDTO model)
		{
			await _feedbackService.AddFeedBackWithLanguage( model, subscriptionId);

			return Ok();
		}
	}
}
