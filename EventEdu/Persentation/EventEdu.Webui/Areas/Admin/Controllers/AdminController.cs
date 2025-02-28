using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
