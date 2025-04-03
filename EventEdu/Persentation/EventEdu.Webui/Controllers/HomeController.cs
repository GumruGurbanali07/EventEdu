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

	public HomeController(ILogger<HomeController> logger, IHeroSectionService heroSectionService, IStringLocalizer<HomeController> localizer)
	{
		_logger = logger;
		_localizer = localizer;
		_heroSectionService = heroSectionService;

	}



	public async Task<IActionResult> Index()
	{
		var lang = HttpContext.Session.GetString("lang") ?? "en-US";
		var slider = await _heroSectionService.GetHeroSectionAll();

		var vm = new HomeIndexVM()
		{
			HeroSectionDetails = slider.Item2,
			HeroSections = slider.Item1
		};


		ViewBag.Localizer = _localizer;

		return View(vm);
	}



}