
using EventEdu.Application.DTOs.Language;
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


	[HttpGet]
	public IActionResult Change(string? lang)
	{
		if (!string.IsNullOrEmpty(lang))
		{
			HttpContext.Session.SetString("lang", lang);
		}

		return RedirectToAction("Index", "Home");
	}


	[HttpPost("create")]
	public async Task<IActionResult> CreateLanguage([FromBody] CreateLanguageDTO languageDTO)
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
	[HttpGet("{isoCode}")]
	public async Task<IActionResult> Get(string isoCode)
	{
		try
		{
			var language = await _languageService.GetLanguageAsync(isoCode);
			return Ok(language);
		}
		catch (Exception ex)
		{
			return StatusCode(500, "Daxili server xətası: " + ex.Message);
		}
	}

	[HttpGet("getall")]
	public async Task<IActionResult> GetAll()
	{
		try
		{
			var languages = await _languageService.GetLanguagesAsync();
			return Ok(languages);
		}
		catch (Exception ex)
		{
			return StatusCode(500, "Daxili server xətası: " + ex.Message);
		}
	}

	[HttpPut("{id}")]
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

	[HttpDelete("{id}")]
	public async Task<IActionResult> SoftDelete(Guid id)
	{
		try
		{
			await _languageService.SoftDeleteLanguageAsync(id);
			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, "Daxili server xətası: " + ex.Message);
		}
	}

	[HttpPost("restore/{id}")]
	public async Task<IActionResult> Restore(Guid id)
	{
		try
		{
			await _languageService.RestoreLanguageAsync(id);
			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, "Daxili server xətası: " + ex.Message);
		}
	}



}
