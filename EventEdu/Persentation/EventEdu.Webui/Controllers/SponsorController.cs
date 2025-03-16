using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers;

public class SponsorController : Controller
{
    public async Task<IActionResult> Index()
    {
        //var sponsors = await ISponsorService.GetAllSponsorsByLanguageAsync("en");
        //return View(sponsors);
        return View();
    }
}