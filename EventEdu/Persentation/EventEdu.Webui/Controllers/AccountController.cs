using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult ParticipationHistory()
        {
            return View();
        }
    }
}
