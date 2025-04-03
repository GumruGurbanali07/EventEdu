using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Services;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AboutSectionController : Controller
    {
        private readonly IAboutSectionService _aboutSectionService;
        private readonly ILanguageService _languageService;
        private readonly IHttpContextAccessor _contextAccessor;
		public AboutSectionController(IAboutSectionService aboutSectionService, ILanguageService languageService, IHttpContextAccessor contextAccessor)
		{
			_aboutSectionService = aboutSectionService;
			_languageService = languageService;
			_contextAccessor = contextAccessor;
		}
		public async Task<IActionResult> Index()
        {
            var aboutSection = await _aboutSectionService.GetAboutSectionAll();

            var vm = new AboutIndexVM()
            {
                AboutSection = aboutSection.Item1,
                AboutSectionDetail = aboutSection.Item2
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddAboutSectionAsync()
        {
			var languages = await _languageService.GetLanguagesAsync();


			

			ViewBag.Languages = languages.Select(a => new SelectListItem()
			{
				Value = a.Id.ToString(),
				Text = a.Name
			});
			return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAboutSection(CreateAboutSectionDTO addAboutSectionDTO)
        {
            if (!ModelState.IsValid)
            {
				var languages = await _languageService.GetLanguagesAsync();

				if (languages == null || !languages.Any())
				{
					ModelState.AddModelError("", "No languages found. Please add languages first.");
				}

				ViewBag.Languages = languages.Select(a=> new SelectListItem()
                {
					Value = a.Id.ToString(),
					Text = a.Name
				});
			}
           
                await _aboutSectionService.AddAboutSection(addAboutSectionDTO);

                return RedirectToAction("Index", "AboutSection");
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
                var aboutSection = await _aboutSectionService.GetAboutSectionById(id);

               
				var languages = await _languageService.GetLanguagesAsync();

				if (languages == null || !languages.Any())
				{
					ModelState.AddModelError("", "No languages found. Please add languages first.");
				}

				ViewBag.Languages = languages.Select(a => new SelectListItem()
				{
					Value = a.Id.ToString(),
					Text = a.Name
				});
			

				var updateAboutSectionDTO = new CreateAboutSectionDTO
                {
                    Title = aboutSection.Item2.Title,
                    Description = aboutSection.Item2.Description,
                    ImagePath = aboutSection.Item1.ImagePath,
                    LanguageId = aboutSection.Item2.LanguageId
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
        public async Task<IActionResult> EditAboutSection(CreateAboutSectionDTO updateAboutSectionDTO)
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
					Value = a.Id.ToString(),
					Text = a.Name
				});
				return View(updateAboutSectionDTO);
            }

            try
            {
                await _aboutSectionService.EditAboutSection(updateAboutSectionDTO.Id, updateAboutSectionDTO);
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
