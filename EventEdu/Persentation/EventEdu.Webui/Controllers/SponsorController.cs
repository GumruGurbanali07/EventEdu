using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers;

public class SponsorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}