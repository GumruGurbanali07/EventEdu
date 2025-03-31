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
	private readonly ICategoryService _categoryService;

	public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer, ICategoryService categoryService)
	{
		_logger = logger;
		_localizer = localizer;
		_categoryService = categoryService;
	}
	
	public async Task<IActionResult> Index()
	{
		var lang = HttpContext.Session.GetString("lang") ?? "en-US";

		var categories = await _categoryService.GetCategoriesByLanguageAsync(lang);
		
		ViewBag.Localizer = _localizer ;


		var vm = new HomeIndexVM()
		{
			Categories = categories.Item2

		};


        return View(vm);
	}


}