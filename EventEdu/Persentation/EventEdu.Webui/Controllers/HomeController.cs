using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<HomeController> _localizer;
    private readonly ICategoryService _categoryService;
    private readonly IHeroSesc

    public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }



    public IActionResult Index()
    {
        ViewBag.Localizer = _localizer;

        return View();
    }
}