using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;
[ApiController]
[Route("api/[controller]")]
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
	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var isoCode = "az-AZ"; 
		var categories = await _categoryService.GetCategoriesByLanguageAsync(isoCode);

		
		ViewBag.Localizer = _localizer;
		ViewBag.Categories = categories;

		return View();
	}


}