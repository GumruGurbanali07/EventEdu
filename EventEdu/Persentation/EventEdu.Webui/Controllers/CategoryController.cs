using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace EventEdu.Webui.Controllers
{

	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly ILanguageService _languageService;

		public CategoryController(ICategoryService categoryService, ILanguageService languageService = null)
		{
			_categoryService = categoryService;
			_languageService = languageService;
		}

		
		

		public async Task<IActionResult> GetCategoryByIdAndLanguage(Guid categoryId, string isoCode)
		{
			var category = await _categoryService.GetCategoryByIdAndLanguageAsync(categoryId, isoCode);
			var language = await  _languageService.GetLanguagesAsync();
			ViewBag.Language = language.Select(a => new SelectListItem()
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
			var languageId = await _languageService.GetLanguageAsync(category.IsoCode);

			if (category == null)
				return NotFound("Category not found");

			return Ok(category);
		}

	
		
		
	
		
		
	


	}
}
