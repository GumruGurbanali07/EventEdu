using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

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