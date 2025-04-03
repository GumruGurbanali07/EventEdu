using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AboutSectionController : Controller
    {
        private readonly IAboutSectionService _aboutSectionService;
        private readonly AppDbContext _context;
        public AboutSectionController(IAboutSectionService aboutSectionService, AppDbContext context)
        {
           _aboutSectionService = aboutSectionService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var aboutSection = await _aboutSectionService.GetAllAboutSectionsAsync();
            return View(aboutSection);
        }

        [HttpGet]
        public IActionResult AddAboutSection()
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
        public async Task<IActionResult> AddAboutSection(CreateAboutSectionDTO addAboutSectionDTO)
        {
            try
            {
                await _aboutSectionService.AddAboutSection(addAboutSectionDTO);

                return RedirectToAction("Index", "AboutSection");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addAboutSectionDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAboutSection(Guid Id)
        {
            try
            {
                await _aboutSectionService.DeleteAboutSection(Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreAboutSection(Guid id)
        {
            try
            {
                await _aboutSectionService.RestoreAboutSection(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAboutSection(Guid id)
        {
            try
            {
                var aboutSection = await _aboutSectionService.GetAboutSectionById(id, "az-AZ");

                if (aboutSection == null)
                {
                    return NotFound("About section not found.");
                }

                var updateAboutSectionDTO = new CreateAboutSectionDTO
                {
                    Title = aboutSection.Title,
                    Description = aboutSection.Description,
                    ImagePath = aboutSection?.ImagePath,
                    LanguageId = aboutSection.LanguageId
                };


                return View(updateAboutSectionDTO);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAboutSection(Guid id, [FromForm] CreateAboutSectionDTO updateAboutSectionDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(updateAboutSectionDTO);
            }

            try
            {
                await _aboutSectionService.EditAboutSection(id, updateAboutSectionDTO);
                TempData["Success"] = "About Section updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(updateAboutSectionDTO);
            }
        }

    }
}
