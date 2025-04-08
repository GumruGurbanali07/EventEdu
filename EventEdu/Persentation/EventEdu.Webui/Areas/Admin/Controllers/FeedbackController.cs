using EventEdu.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
	{
		readonly private IFeedbackService _feedbackService;
public FeedbackController(IFeedbackService feedbackService)
		{
			_feedbackService = feedbackService;
		}

		public async Task<IActionResult> Index()
		{
			var f = await _feedbackService.GetFeedbackAsync();
			return View(f);
		}
	}
}
