using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Webui.Areas.Admin.Controllers;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IStringLocalizer<HomeController> _localizer;
	private readonly IHeroSectionService _heroSectionService;
    private readonly ICategoryService _categoryService;
    private readonly IEventService _eventService;

    public HomeController(ILogger<HomeController> logger, IHeroSectionService heroSectionService, IStringLocalizer<HomeController> localizer, ICategoryService categoryService,
        IEventService eventService)
	{
		_logger = logger;
		_localizer = localizer;
		_heroSectionService = heroSectionService;
		_categoryService = categoryService;
		_eventService = eventService;
	}



	public async Task<IActionResult> Index()
	{
		var lang = HttpContext.Session.GetString("lang") ?? "en-US";
		var slider = await _heroSectionService.GetHeroSectionAll();
		var category = await _categoryService.GetCategoriesAllAsync();
		var events = await _eventService.GetEventAll();

		ViewBag.AlertMessage = TempData["Success"];
		var vm = new HomeIndexVM()
		{
			HeroSectionDetails = slider.Item2,
			HeroSections = slider.Item1,
			CategoryDetails = category,
			Events = events,
			//Categories = category
		};


		ViewBag.Localizer = _localizer;

		return View(vm);
	}



}