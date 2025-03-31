using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;
//[ApiController]
//[Route("api/[controller]")]
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

        // HomeIndexVM modelini doldururuq
        var viewModel = new HomeIndexVM
        {
            AboutSection = aboutSection,
            Sponsors = sponsors,
        };

        return View(viewModel);
    }



}