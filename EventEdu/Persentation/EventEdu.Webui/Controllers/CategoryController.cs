using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EventEdu.Webui.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : Controller
    {
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
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
