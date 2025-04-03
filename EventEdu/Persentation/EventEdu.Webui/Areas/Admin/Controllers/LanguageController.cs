using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
public class LanguageController : Controller
{
        readonly private ILanguageService _languageService;

    public LanguageController(ILanguageService languageService)
    {
        _languageService = languageService;
    }

		public async Task<IActionResult> Index( string lang)
    {
        if (!string.IsNullOrEmpty(lang))
        {
            HttpContext.Session.SetString("lang", lang);
        }

            var language = await _languageService.GetLanguagesAsync();
            return View(language);
    }

        public IActionResult Create()
            => View();

    [HttpPost]
        public async Task<IActionResult> Create(CreateLanguageDTO createLanguageDTO)
    {
            if (!ModelState.IsValid) return View(createLanguageDTO);

            await _languageService.CreateAsync(createLanguageDTO);

            return Redirect(nameof(Index));


        }
       
    
  


    [HttpGet]
		public async Task<IActionResult>  Edit (Guid id)
    {
            try { 
            var language = await _languageService.GetLanguageById(id);

            if (language == null) return NotFound();

            var ud = new UpdateLanguageDTO
            {
                Name = language.Name,
                IsoCode = language.IsoCode,
                ImagePath = language.ImagePath
            };
            return View(ud);

        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
		public async Task<IActionResult> Edit(Guid id, UpdateLanguageDTO updateLanguageDTO)
        {
			
			if (!ModelState.IsValid) return View(updateLanguageDTO);

            await _languageService.UpdateLanguageAsync(id, updateLanguageDTO);
			return Redirect(nameof(Index));
    }

    [HttpPost]
		public async Task<IActionResult> Delete(Guid id)
        {
            await _languageService.SoftDeleteLanguageAsync(id);
			return Redirect(nameof(Index));
        }
    }



}
