using EventEdu.Application.DTOs.Event;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly AppDbContext _context;
        readonly private ILanguageService _languageService;
        readonly private ICategoryService _categoryService;
        readonly private ISponsorService _sponsorService;
        readonly private ISpeakerService _speakerService;

        public EventController(IEventService eventService, AppDbContext context, ILanguageService languageService, ICategoryService categoryService, ISpeakerService speakerService, ISponsorService sponsorService)
        {
            _eventService = eventService;
            _languageService = languageService;
            _categoryService = categoryService;
            _speakerService = speakerService;
            _sponsorService = sponsorService;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var languages = _context.Languages.ToList();

            var events = await _eventService.GetEventDetailAll();

            return View(events);
        }

        public async Task<IActionResult> Details(string id)
        {
            var events = await _eventService.GetEventById(id);

            return View(events);
        }

        public async Task<IActionResult> Create()
        {
            var getLanguage = await _languageService.GetLanguagesAsync();
            ViewBag.Language = getLanguage.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });
            var sponsor = await _sponsorService.GetSponsorAll();

            ViewBag.Sponsor = sponsor.Item2;
            var category = await _categoryService.GetCategoriesAllAsync();
            ViewBag.Category = category.Select(a => new SelectListItem()
            {
                Text = a.CategoryName,
                Value = a.Id.ToString()
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventDTO createEventDTO)
        {
            if (!ModelState.IsValid)
            {

                var getLanguage = await _languageService.GetLanguagesAsync();
                ViewBag.Language = getLanguage.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });
                var category = await _categoryService.GetCategoriesAllAsync();
                ViewBag.Category = category.Select(a => new SelectListItem()
                {
                    Text = a.CategoryName,
                    Value = a.Id.ToString()
                });

                var sponsor = await _sponsorService.GetSponsorAll();
                var speak = await _speakerService.GetSpeakersAllAsync();
                ViewBag.Sponsor = sponsor.Item2;
                ViewBag.Speak = speak.Item2;
                return View(createEventDTO);
            }
            await _eventService.AddEventWithLanguageAsync(createEventDTO);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var getLanguage = await _languageService.GetLanguagesAsync();
            ViewBag.Language = getLanguage.Select(a => new SelectListItem()
            {
                Text = a.Name,
                Value = a.Id.ToString()
            });
            var category = await _categoryService.GetCategoriesAllAsync();
            ViewBag.Category = category.Select(a => new SelectListItem()
            {
                Text = a.CategoryName,
                Value = a.Id.ToString()
            });

            var events = await _eventService.GetEventById(Id);
            var eventDetails = await _eventService.GetEventDetailsById(events.Id.ToString());
            var eu = new UpdateEventDTO()
            {
                CategoryId = events.CategoryId,
                Id = events.Id,
                ImageUrl = events.ImageUrl,
                EndDate = events.EndDate,
                StartDate = events.StartDate,
                LanguageId = eventDetails.LanguageId,
                Title = eventDetails.Title,
                Description = eventDetails.Description,

            };
            return View(events);
        }

		[HttpPost]
        public async Task<IActionResult> Edit(Guid Id, UpdateEventDTO updateEventDTO)
        {
            if (!ModelState.IsValid)
            {
                var getLanguage = await _languageService.GetLanguagesAsync();
                ViewBag.Language = getLanguage.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });
                var category = await _categoryService.GetCategoriesAllAsync();
                ViewBag.Category = category.Select(a => new SelectListItem()
                {
                    Text = a.CategoryName,
                    Value = a.Id.ToString()
                }); ;
                return View(updateEventDTO);
            }
            await _eventService.UpdateEventAsync(Id, updateEventDTO);

            return Redirect(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(Guid Id)
        {
            await _eventService.RestoreEventAsync(Id);
            return Redirect(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await _eventService.SoftDeleteEventAsync(Id);
            return Redirect(nameof(Index));
        }
    }
}