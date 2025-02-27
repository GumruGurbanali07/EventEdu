using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
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

	[HttpGet]
	public async Task<IActionResult> Index()
	=> await _languageService.GetAllLanguagesAsync().ContinueWith(a => Ok(a.Result));

	[HttpPost]
	public async Task<IActionResult> AddLanguage([FromBody] LanguageGetDTO languageAddDTO)
	{
		if (languageAddDTO == null)
		{
			return BadRequest("\"Language information was not entered correctly.\"");
		}
		try
		{
			var result = await _languageService.AddLanguageAsync(languageAddDTO);
			if (result)
			{
				return Ok("Language added successfully");
			}
			return BadRequest("Language could not be added");
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Xəta baş verdi: {ex.Message}");
		}
	}
}