using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Webui.ViewsModels;
using EventEdu.Webui.Areas.Admin.Controllers;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStringLocalizer<HomeController> _localizer;
    private readonly ICategoryService _categoryService;
    private readonly IAboutSectionService _aboutSectionService;
    private readonly ISponsorService _sponsorService;

    public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer, ICategoryService categoryService,
        IAboutSectionService aboutSectionService, ISponsorService sponsorService)
    {
        _logger = logger;
        _localizer = localizer;
        _categoryService = categoryService;
        _aboutSectionService = aboutSectionService;
        _sponsorService = sponsorService;
    }
    //[HttpGet]
    public async Task<IActionResult> Index()
    {
        var lang = HttpContext.Session.GetString("lang") ?? "en-US";

        var categories = await _categoryService.GetCategoriesByLanguageAsync(lang);
        var aboutSection = await _aboutSectionService.GetAllAboutSectionsAsync();
        var sponsors = await _sponsorService.GetAllSponsorsByLanguageAsync(lang);

        ViewBag.Localizer = _localizer;
        ViewBag.Categories = categories;

        var viewModel = new HomeIndexVM
        {
            Categories = categories.Item2,
            AboutSection = aboutSection,
            Sponsors = sponsors,
        };

        return View(viewModel);
    }

}