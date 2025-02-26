using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

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

		[HttpPost("add-category")]
		public async Task<IActionResult> AddCategory([FromBody] Category category)
		{
			if (category == null)
			{
				return BadRequest("Category cannot be null");
			}

			var result = await _categoryService.AddCategoryAsync(category);
			if (result)
			{
				return Ok("Category added successfully");
			}

			return BadRequest("Failed to add category");
		}

		[HttpPost("add-category-detail")]
		public async Task<IActionResult> AddCategoryDetail([FromBody] CategoryDetail categoryDetail)
		{
			if (categoryDetail == null)
			{
				return BadRequest("Category detail cannot be null");
			}

			try
			{
				var result = await _categoryService.AddCategoryDetailAsync(categoryDetail);
				if (result)
				{
					return Ok("Category detail added successfully");
				}
				return BadRequest("Failed to add category detail");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("get-category-details/{categoryId}/{languageId}")]
		public async Task<IActionResult> GetCategoryDetailsByLanguage(Guid categoryId, Guid languageId)
		{
			var categoryDetails = await _categoryService.GetCategoryDetailsByLanguageAsync(categoryId, languageId);
			if (categoryDetails == null)
			{
				return NotFound("Category details not found");
			}
			return Ok(categoryDetails);
		}
	}
}
