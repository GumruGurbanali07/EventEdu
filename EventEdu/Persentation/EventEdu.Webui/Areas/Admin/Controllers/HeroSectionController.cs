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
        public async Task<IActionResult> DeleteSponsor(Guid id)
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
        public async Task<IActionResult> RestoreSponsor(Guid id)
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

        public IActionResult EditSlider()
        {
            return View();
        }
    }
}
