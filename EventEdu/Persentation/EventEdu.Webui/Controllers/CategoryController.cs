using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace EventEdu.Webui.Controllers
{
	
	public class CategoryController : Controller
    {
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		
		public async Task<IActionResult> GetCategoriesByLanguage(string isoCode)
		{
			var categories = await _categoryService.GetCategoriesByLanguageAsync(isoCode);
			return Ok(categories);
		}
				
		public async Task<IActionResult> AddCategoryWithLanguage( CategoryDetail categoryDetail)
		{
			await _categoryService.AddCategoryWithLanguageAsync(categoryDetail.CategoryName, categoryDetail.LanguageId);
			return Ok(new { message = "Category added successfully" });
		}

	}
}
