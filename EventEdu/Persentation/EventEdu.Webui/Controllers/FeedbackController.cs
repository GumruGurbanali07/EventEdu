using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{
	
	public class FeedbackController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
