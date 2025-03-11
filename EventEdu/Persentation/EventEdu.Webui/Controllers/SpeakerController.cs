using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.Services;
using EventEdu.Persistence.Services;
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
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPut("{speakerId}")]
		public async Task<IActionResult> UpdateSpeaker(Guid speakerId, [FromBody] UpdateSpeakerDTO updateSpeakerDTO)
		{
			if (updateSpeakerDTO == null)
			{
				return BadRequest("Invalid speaker update data.");
			}

			await _speakerService.UpdateSpeakerAsync(speakerId, updateSpeakerDTO);
			return Ok("Speaker successfully updated.");
		}

		[HttpDelete("soft-delete/{speakerId}")]
		public async Task<IActionResult> SoftDeleteSpeaker(Guid speakerId)
		{
			try
			{
				await _speakerService.SoftDeleteSpeakerAsync(speakerId);
				return Ok(new { message = "Speaker soft deleted successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		
		[HttpPut("restore/{speakerId}")]
		public async Task<IActionResult> RestoreSpeaker(Guid speakerId)
		{
			try
			{
				await _speakerService.RestoreSpeakerAsync(speakerId);
				return Ok(new { message = "Speaker restored successfully." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

	}
}
