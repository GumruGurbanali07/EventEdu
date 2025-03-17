using EventEdu.Application.DTOs.Event;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly AppDbContext _context;

        public EventController(IEventService eventService, AppDbContext context)
        {
            _eventService = eventService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AddEvent()
        {
            var languages = _context.Languages.ToList();

            if (languages == null || !languages.Any())
            {
                ModelState.AddModelError("", "No languages found. Please add languages first.");
            }

            ViewBag.Languages = languages;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([FromForm] CreateEventDTO createEventDTO)
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
        [HttpGet]
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
        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> UpdateEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEvent(Guid eventId, [FromForm] UpdateEventDTO updateEventDTO)
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
        [HttpPost]
        public async Task<IActionResult> SoftDeleteEventAsync(Guid eventId)
        {
            try
            {
                await _eventService.SoftDeleteEventAsync(eventId);
                return RedirectToAction("GetEventsByLanguage");
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

        [HttpPost]
        public async Task<IActionResult> RestoreEventAsync(Guid eventId)
        {
            try
            {
                await _eventService.RestoreEventAsync(eventId);
               return RedirectToAction("GetEventsByLanguage");
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
    }
}
