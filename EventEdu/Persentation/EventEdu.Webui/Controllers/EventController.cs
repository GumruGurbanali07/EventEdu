using EventEdu.Application.DTOs.Event;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : Controller
{
	private readonly IEventService _eventService;

	public EventController(IEventService eventService)
	{
		_eventService = eventService;
	}
	[HttpPost("add")]
	public async Task<IActionResult> AddEvent([FromBody] CreateEventDTO createEventDTO)
	{
		try
		{
			await _eventService.AddEventWithLanguageAsync(createEventDTO);
			return Ok(new { message = "Event successfully added." });
		}

		catch (Exception ex)
		{
			return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
		}
	}
	[HttpGet("get-by-language/{isoCode}")]
	public async Task<IActionResult> GetEventsByLanguage(string isoCode)
	{
		try
		{
			var events = await _eventService.GetEventsByLanguageAsync(isoCode);
			if (events == null || events.Count == 0)
			{
				return NotFound(new { message = "Bu dil üçün heç bir tədbir tapılmadı." });
			}
			return Ok(events);
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Serverdə xəta baş verdi.", error = ex.Message });
		}
	}
	[HttpGet("{eventId}/{isoCode}")]
	public async Task<IActionResult> GetEventByIdAndLanguage(Guid eventId, string isoCode)
	{
		try
		{
			// Call the service to get the event by ID and language code
			var eventDetails = await _eventService.GetEventByIdAndLanguageAsync(eventId, isoCode);

			if (eventDetails == null)
			{
				return NotFound("Event not found.");
			}

			return Ok(eventDetails);
		}
		catch (Exception ex)
		{
			// Handle any errors, and return a server error if necessary
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
	[HttpPut("{eventId}")]
	public async Task<IActionResult> UpdateEvent(Guid eventId, [FromBody] UpdateEventDTO updateEventDTO)
	{
		try
		{
			// Call the service layer to update the event
			await _eventService.UpdateEventAsync(eventId, updateEventDTO);

			// Return a NoContent response indicating the update was successful
			return NoContent();
		}

		catch (Exception ex)
		{
			// Catch any unexpected errors and return 500 (Internal Server Error)
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
		[HttpDelete("soft-delete/{eventId}")]
	   public async Task<IActionResult> SoftDeleteEventAsync(Guid eventId)
		{
			try
			{
				await _eventService.SoftDeleteEventAsync(eventId);
				return Ok(new { message = "Event has been soft deleted successfully." });
			}
			catch (NotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while soft deleting the event.", details = ex.Message });
			}
		}

		// Restore Event
		[HttpPost("restore/{eventId}")]
		public async Task<IActionResult> RestoreEventAsync(Guid eventId)
		{
			try
			{
				await _eventService.RestoreEventAsync(eventId);
				return Ok(new { message = "Event has been restored successfully." });
			}
			catch (NotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while restoring the event.", details = ex.Message });
			}
		}

		//public IActionResult Index()
		//{
		//    return View();
		//}

		//public IActionResult EventDetail()
		//{
		//    return View();
		//}

		//public IActionResult Technology()
		//{
		//    return View();
		//}

		//public IActionResult Academic()
		//{
		//    return View();
		//}

		//public IActionResult Career()
		//{
		//    return View();
		//}

		//public IActionResult Art()
		//{
		//    return View();
		//}

		//public IActionResult Sports()
		//{
		//    return View();
		//}
	}



