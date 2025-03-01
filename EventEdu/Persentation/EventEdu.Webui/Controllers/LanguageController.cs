
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
namespace EventEdu.Webui.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageController : Controller
{
	private readonly ILanguageService _languageService;

	public LanguageController(ILanguageService languageService)
	{
		_languageService = languageService;
	}

	[HttpPost]
	public async Task<IActionResult> AddLanguage([FromBody] LanguageViewModel languageViewModel)
	{
		if (string.IsNullOrEmpty(languageViewModel.Name) || string.IsNullOrEmpty(languageViewModel.IsoCode))
		{
			return BadRequest(new { message = "Name və ISO Code boş ola bilməz" });
		}

		await _languageService.AddLanguageAsync(languageViewModel);
		return Ok(new { message = "Language added successfully" });
	}

	public IActionResult Change(string? lang)
	{
		if (!string.IsNullOrEmpty(lang))
		{
			HttpContext.Session.SetString("lang", lang);
		}

		return RedirectToAction("Index", "Home");
	}
}
