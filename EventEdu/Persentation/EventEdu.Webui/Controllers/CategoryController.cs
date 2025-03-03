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

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategoriesByLanguage(string isoCode)
		{
			var categories = await _categoryService.GetCategoriesByLanguageAsync(isoCode);
			return Ok(categories);
		}

		//[HttpGet]
		//public async Task<IActionResult> Index(string lang)
		//{
		//	var categories = await _categoryService.GetCategoriesByLanguageAsync(lang);
		//	return View(categories);
		//}

		[HttpPost]
		public async Task<IActionResult> AddCategoryWithLanguage([FromBody] CreateCategoryDTO createCategoryDTO)
		{
			if (createCategoryDTO == null)
			{
				return BadRequest("Category data is required.");
			}

			try
			{
				await _categoryService.AddCategoryWithLanguageAsync(createCategoryDTO);
				return Ok("Category added successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest($"Error: {ex.Message}");
			}
		}

	}
}
