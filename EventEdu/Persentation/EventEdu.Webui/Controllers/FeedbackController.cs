using EventEdu.Application.DTOs.Feedback;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class FeedbackController : Controller
	{

		readonly  private IFeedbackService _feedbackService;

		public FeedbackController(IFeedbackService feedbackService)
		{
			_feedbackService = feedbackService;
		}

		[HttpGet("{eventId}")]
		public async Task<IActionResult> Index(Guid eventId)
		{
		    var feedBack = await _feedbackService.GetFeedbacksByEventAndLanguageAsync(eventId);
			return Ok(feedBack);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] AddFeedBackDTO model)
		{
			await _feedbackService.AddFeedBackWithLanguage( model);

			return Ok();
		}
	}
}
