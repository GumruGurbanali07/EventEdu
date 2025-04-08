using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HeroSectionController : Controller
	{
		private readonly IHeroSectionService _sliderService;
		private readonly ILanguageService _languageService;

		public HeroSectionController(IHeroSectionService sliderService,
ILanguageService languageService)
		{
			_sliderService = sliderService;
			_languageService = languageService;
		}


		public async Task<IActionResult> Index()
		{
			var sliders = await _sliderService.GetAllSlidersAsync();
			return View(sliders);
		}

		[HttpGet]
		public async Task<IActionResult> AddSlider()
		{
			var languages = await _languageService.GetLanguagesAsync();

			if (languages == null || !languages.Any())
			{
				ModelState.AddModelError("", "No languages found. Please add languages first.");
			}

			ViewBag.Languages = languages.Select(a => new SelectListItem()
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddSlider(CreateHeroSectionDTO addSliderDTO)
		{
			if (!ModelState.IsValid)
			{
				var languages = await _languageService.GetLanguagesAsync();

				if (languages == null || !languages.Any())
				{
					ModelState.AddModelError("", "No languages found. Please add languages first.");
				}

				ViewBag.Languages = languages.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
				return View(addSliderDTO);
			}
			try
			{
				await _sliderService.AddSlider(addSliderDTO);

				return RedirectToAction("Index", "HeroSection");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(addSliderDTO);
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteSlider(Guid id)
		{
			try
			{
				await _sliderService.DeleteSlider(id);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		public async Task<IActionResult> RestoreSlider(Guid id)
		{
			try
			{
				await _sliderService.RestoreSlider(id);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet]
		public async Task<IActionResult> EditSlider(Guid id)
		{
			try
			{
				var languages = await _languageService.GetLanguagesAsync();

				if (languages == null || !languages.Any())
				{
					ModelState.AddModelError("", "No languages found. Please add languages first.");
				}

				ViewBag.Languages = languages.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
				var slider = await _sliderService.GetSLiderById(id, "az-AZ");

				if (slider == null)
				{
					return NotFound("Slider not found.");
				}

				var updateSliderDTO = new CreateHeroSectionDTO
				{
					Title = slider.Title,
					Description = slider.Description,
					ImagePath = slider.ImagePath,
					LanguageId = slider.LanguageId
				};

				return View(updateSliderDTO);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public async Task<IActionResult> EditSlider(Guid id, [FromForm] CreateHeroSectionDTO updateSliderDTO)
		{

			if (!ModelState.IsValid)
			{
				var languages = await _languageService.GetLanguagesAsync();

				if (languages == null || !languages.Any())
				{
					ModelState.AddModelError("", "No languages found. Please add languages first.");
				}

				ViewBag.Languages = languages.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});
				return View(updateSliderDTO);
			}

			try
			{
				await _sliderService.EditSlider(id, updateSliderDTO);
				TempData["Success"] = "Slider updated successfully!";
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return View(updateSliderDTO);
			}
		}

	}
}
