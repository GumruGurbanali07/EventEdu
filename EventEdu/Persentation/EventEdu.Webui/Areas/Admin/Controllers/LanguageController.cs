using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
namespace EventEdu.Webui.Areas.Admin.Controllers;

//[ApiController]
//[Route("api/[controller]")]
[Area("Admin")]
public class LanguageController : Controller
{
    private readonly ILanguageService _languageService;

    public LanguageController(ILanguageService languageService)
    {
        _languageService = languageService;
    }


    [HttpGet]
    public IActionResult Change(string? lang)
    {
        if (!string.IsNullOrEmpty(lang))
        {
            HttpContext.Session.SetString("lang", lang);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> CreateLanguage([FromForm] CreateLanguageDTO languageDTO)
    {
        try
        {
            var newLanguage = await _languageService.CreateAsync(languageDTO);
            return Ok(newLanguage);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Daxili server xətası: " + ex.Message);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Get(string isoCode)
    {
        try
        {
            var language = await _languageService.GetLanguagesAsync();
            return Ok(language);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Daxili server xətası: " + ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var languages = await _languageService.GetLanguagesAsync();
            return View(languages);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Daxili server xətası: " + ex.Message);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        try
        {
            var aboutSection = await _languageService.GetLanguageAsync( "az-AZ");

            if (aboutSection == null)
            {
                return NotFound("Language not found.");
            }

            var updateLanguageDTO = new UpdateLanguageDTO
            {
             Name = aboutSection.Name,
             IsoCode = aboutSection.IsoCode,
             ImagePath = aboutSection.ImagePath
            };


            return View(updateLanguageDTO);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLanguageDTO languageDTO)
    {
        try
        {
            await _languageService.UpdateLanguageAsync(id, languageDTO);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Daxili server xətası: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        try
        {
            await _languageService.SoftDeleteLanguageAsync(id);
            return RedirectToAction("GetAll");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Daxili server xətası: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Restore(Guid id)
    {
        try
        {
            await _languageService.RestoreLanguageAsync(id);
            return RedirectToAction("GetAll");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Daxili server xətası: " + ex.Message);
        }
    }



}
