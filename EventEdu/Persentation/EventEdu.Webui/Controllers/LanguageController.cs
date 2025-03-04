
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
namespace EventEdu.Webui.Controllers;

//[ApiController]
//[Route("api/[controller]")]
public class LanguageController : Controller
{
	private readonly ILanguageService _languageService;

	public LanguageController(ILanguageService languageService)
	{
		_languageService = languageService;
	}

	//[HttpPost("create")]
	public async Task<IActionResult> CreateLanguage([FromBody] CreateLanguageDTO languageDTO)
	{
		try
		{
			var newLanguage = await _languageService.CreateAsync(languageDTO);
			return Ok(newLanguage); // 200 OK cavabı ilə yeni dil məlumatını qaytarır.
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(ex.Message); // 400 Bad Request cavabı ilə xəta mesajını qaytarır.
		}
		catch (Exception ex)
		{
			return StatusCode(500, "Daxili server xətası: " + ex.Message); // 500 Internal Server Error cavabı ilə ümumi xəta mesajını qaytarır.
		}
	}

	//[HttpPost]
	public async Task<IActionResult> AddLanguage([FromBody] LanguageViewModel languageViewModel)
	{
		if (string.IsNullOrEmpty(languageViewModel.Name) || string.IsNullOrEmpty(languageViewModel.IsoCode))
		{
			return BadRequest(new { message = "Name və ISO Code boş ola bilməz" });
		}

		await _languageService.AddLanguageAsync(languageViewModel);
		return Ok(new { message = "Language added successfully" });
	}

	//[HttpGet]
	public IActionResult Change(string? lang)
	{
		if (!string.IsNullOrEmpty(lang))
		{
			HttpContext.Session.SetString("lang", lang);
		}

		return RedirectToAction("Index", "Home");
	}
}
