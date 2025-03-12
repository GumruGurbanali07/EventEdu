using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace EventEdu.Webui.Controllers
{
	//[ApiController]
	//[Route("api/[controller]")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet("GetCategories")]
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
		[HttpGet("GetCategoryById")]
		public async Task<IActionResult> GetCategoryByIdAndLanguage(Guid categoryId, string isoCode)
		{
			var category = await _categoryService.GetCategoryByIdAndLanguageAsync(categoryId, isoCode);

			if (category == null)
				return NotFound("Category not found");

			return Ok(category);
		}

		[HttpPost("AddCategory")]
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

		[HttpPut("{categoryId}")]
		public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] UpdateCategoryDTO updateCategoryDTO)
		{
			try
			{
				await _categoryService.UpdateCategoryAsync(categoryId, updateCategoryDTO);
				return Ok(new { message = "Category updated successfully." });
			}
			catch(Exception ex)
			{
				return BadRequest(new { error = ex.Message });

			}
		}
		[HttpDelete("soft-delete/{categoryId}")]
		public async Task<IActionResult> SoftDeleteCategory(Guid categoryId)
		{
			try
			{
				await _categoryService.SoftDeleteCategoryAsync(categoryId);
				return Ok(new { message = "Category soft deleted successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("restore/{categoryId}")]
		public async Task<IActionResult> RestoreCategory(Guid categoryId)
		{
			try
			{
				await _categoryService.RestoreCategoryAsync(categoryId);
				return Ok(new { message = "Category restored successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}


	}
}
