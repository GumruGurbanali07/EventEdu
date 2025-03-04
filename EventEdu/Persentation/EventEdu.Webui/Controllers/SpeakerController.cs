using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SpeakerController : Controller
    {
		private readonly ISpeakerService _speakerService;

		
		public SpeakerController(ISpeakerService speakerService)
		{
			_speakerService = speakerService;
		}

		[HttpPost("AddSpeakerWithLanguage")]
		public async Task<IActionResult> AddSpeakerWithLanguage([FromBody] CreateSpeakerDTO createSpeakerDTO)
		{
			try
			{
				await _speakerService.AddSpeakerWithLanguageAsync(createSpeakerDTO);
				return Ok("Speaker successfully added.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("language/{isoCode}")]
        public async Task<IActionResult> GetSpeakersByLanguageAsync(string isoCode)
		{
			try
			{
				var speakers = await _speakerService.GetSpeakersByLanguageAsync(isoCode);

				if (speakers == null || speakers.Count == 0)
				{
					return NotFound("No speakers found for the specified language.");
				}

				return Ok(speakers);
			}
			catch (Exception ex)
			{
				// Log the exception (you can use a logging library like Serilog, NLog, etc.)
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
