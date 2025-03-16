using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeroSectionController : Controller
    {
        private readonly IHeroSectionService _sliderService;
        private readonly AppDbContext _context;
        public HeroSectionController(IHeroSectionService sliderService, AppDbContext context)
        {
            _sliderService = sliderService;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllSlidersAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult AddSlider()
        {
            var languages = _context.Languages.ToList();

            if (languages == null || !languages.Any())
            {
                ModelState.AddModelError("", "No languages found. Please add languages first.");
            }

            ViewBag.Languages = languages;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSlider(CreateHeroSectionDTO addSliderDTO)
        {
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
