using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly ILanguageService _languageService;

		public CategoryController(ICategoryService categoryService, ILanguageService languageService)
		{
			_categoryService = categoryService;
			_languageService = languageService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetCategoriesAllAsync();
			return View(categories);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var getLanguage = await _languageService.GetLanguagesAsync();
			ViewBag.Language = getLanguage.Select(a => new SelectListItem
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateCategoryDTO createCategoryDTO)
		{
			if (!ModelState.IsValid)
			{
				var getLanguage = await _languageService.GetLanguagesAsync();
				ViewBag.Language = getLanguage.Select(a => new SelectListItem
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
				return View(createCategoryDTO);
			}

			try
			{
				await _categoryService.AddCategoryWithLanguageAsync(createCategoryDTO);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return View(createCategoryDTO);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Update(Guid categoryId, UpdateCategoryDTO updateCategoryDTO)
		{
			if (!ModelState.IsValid)
			{
				var getLanguage = await _languageService.GetLanguagesAsync();
				ViewBag.Language = getLanguage.Select(a => new SelectListItem
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
				return View(updateCategoryDTO);
			}

			await _categoryService.UpdateCategoryAsync(categoryId, updateCategoryDTO);

			TempData["SuccessMessage"] = "Category successfully updated!";

			return RedirectToAction(nameof(Index));

		}

		[HttpGet]
		public async Task<IActionResult> Update(string Id)
		{
			var category = await _categoryService.GetCategoryById(Guid.Parse(Id));
			if (category == null) return NotFound();

			var getLanguage = await _languageService.GetLanguagesAsync();
			ViewBag.Language = getLanguage.Select(a => new SelectListItem
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});

			var uc = new UpdateCategoryDTO
			{

				CategoryId = category.CategoryId.ToString(),
				CategoryName = category.CategoryName,
				LanguageId = category.LanguageId,
				ImagePath = category.ImagePath
			};

			// ✅ TempData-dan mesajı alıb göstəririk.
			ViewBag.SuccessMessage = TempData["SuccessMessage"];

			return View(uc);
		}

		[HttpPost]
		public async Task<IActionResult> SoftDeleteCategory(Guid Id)
		{
			try
			{
				await _categoryService.SoftDeleteCategoryAsync(Id);

				return RedirectToAction(nameof(Index));

			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> DeleteAboutSection(Guid Id)
		{
			try
			{
				await _categoryService.RestoreCategoryAsync(Id);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
