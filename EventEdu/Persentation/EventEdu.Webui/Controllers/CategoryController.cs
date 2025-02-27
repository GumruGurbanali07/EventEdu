using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace EventEdu.Webui.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : Controller
    {
		private readonly ICategoryService _categoryService;
		private readonly IStringLocalizer<CategoryController> _localizer;
		public CategoryController(ICategoryService categoryService, IStringLocalizer<CategoryController> localizer)
		{
			_categoryService = categoryService;
			_localizer = localizer;
		}

		//[HttpGet("get-categories")]
		//public async Task<IActionResult> GetCategories()
		//{
		//	// Mövcud səhifənin dilini alırıq
		//	string languageCode = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

		//	// Dil koduna uyğun olaraq database-dən dilin ID-sini götürməliyik
		//	Guid languageId = await _categoryService.GetCategoryDetailsByLanguageAsync(languageCode);

		//	// Seçilmiş dilə uyğun kateqoriyaları çək
		//	var categories = await _categoryService.GetCategoryDetailsByLanguageAsync(languageId);

		//	return View(categories);
		//}
		public async Task<IActionResult> Index()
		{
			// Get the current language from the session or request
			var currentLanguage = HttpContext.Session.GetString("lang") ?? "az-AZ";

			// Fetch the LanguageId for the current language
			var languageId = await _categoryService.GetLanguageIdByIsoCodeAsync(currentLanguage);

			if (languageId == Guid.Empty)
			{
				// Handle case where language is not found
				ViewBag.ErrorMessage = _localizer!["LanguageNotFound"];
				return View();
			}

			// Fetch category details for the current language
			var categoryDetails = await _categoryService.GetCategoryDetailsByLanguageAsync(languageId);

			// Pass the localized categories to the view
			ViewBag.Localizer = _localizer;
			ViewBag.Categories = categoryDetails;

			return Ok(); //bax bunlar home index icindekiler ucundurr 
		}


		[HttpPost("add-category-detail")]
		public async Task<IActionResult> AddCategoryDetail([FromBody] CategoryDetailDTO categoryDetail)
		{
			if (categoryDetail == null)
			{
				return BadRequest("Category detail cannot be null");
			}
			
			try
			{
				// Əgər CategoryDetail əlavə edilsə
				var result = await _categoryService.AddCategoryDetailAsync(categoryDetail);

				if (result)
				{
					return Ok("Category detail added successfully");
				}
				else
				{
					return BadRequest("Failed to add category detail: Category might already exist or there is some other issue.");
				}
			}
			catch (Exception ex)
			{
				// Server tərəfdən səhv baş verdikdə
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


	}
}
