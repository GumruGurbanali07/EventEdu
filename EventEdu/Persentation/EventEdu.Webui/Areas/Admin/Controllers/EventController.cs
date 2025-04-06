using EventEdu.Application.DTOs.Event;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]

    public class EventController : Controller
    {
        private readonly IEventService _eventService;
      
		
		readonly private ILanguageService _languageService;
		readonly private ICategoryService _categoryService;
		readonly private ISponsorService _sponsorService;
		readonly private ISpeakerService _speakerService;

		public EventController(IEventService eventService, ILanguageService languageService, ICategoryService categoryService, ISpeakerService speakerService, ISponsorService sponsorService)
        {
            _eventService = eventService;
			_languageService = languageService;
			_categoryService = categoryService;
			_speakerService = speakerService;
			_sponsorService = sponsorService;
        }

	
		public async Task<IActionResult> Index()
            {

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

			var sponsor = await _sponsorService.GetSponsorDetails();

			var speak = await _speakerService.GetSpeakersAsync();
			ViewBag.Speak = speak.Select(a=> new SpeakerDetail() {
			 Id= a.Id,
			 FullName=a.FullName
			}).ToList();

			ViewBag.Sponsor = sponsor.Select(a=> new SponsorDetail()
			{
				Id=a.Id,
				SponsorName=a.SponsorName
			}).ToList();

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
			// Əgər ModelState səhvdirsə, məlumatları yenidən yükləyirik
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

				var sponsor = await _sponsorService.GetSponsorDetails();
				ViewBag.Sponsor = sponsor.Select(a => new SponsorDetail()
				{
					Id = a.Id,
					SponsorName = a.SponsorName
				}).ToList();

				var speak = await _speakerService.GetSpeakersAsync();
				ViewBag.Speak = speak.Select(a => new SpeakerDetail()
				{
					Id = a.Id,
					FullName = a.FullName
				}).ToList();

				// Görünüşə uyğun View göndəririk
				return View(createEventDTO);
			}

			// Əgər ModelState düzgün olsa, əlavə edirik
			try
			{
				await _eventService.AddEventWithLanguageAsync(createEventDTO);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// Hər hansısa bir səhv varsa, səhv mesajı ilə geri dönə bilərsən
				ModelState.AddModelError("", "An error occurred while creating the event: " + ex.Message);
				return View(createEventDTO);
			}
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

			var eventDetails= await _eventService.GetEventDetailsById(Id);
			var events = await _eventService.GetEventById(eventDetails.EventId.ToString());
			
			var (hasSpeaker, hasSponsor) = await _eventService.GetEventSpeakerById(events.Id.ToString());
			ViewBag.EventSponsor = hasSponsor;
			ViewBag.EventSpeaker = hasSpeaker;

				var sponsor = await _sponsorService.GetSponsorDetails();
				ViewBag.Sponsor = sponsor.ToList();

				var speak = await _speakerService.GetSpeakersAsync();
				ViewBag.Speak = speak.ToList();
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

			return View(eu);
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
				});

				var eventDetails = await _eventService.GetEventDetailsById(updateEventDTO.Id.ToString());
				var events = await _eventService.GetEventById(eventDetails.EventId.ToString());

				var (hasSpeaker, hasSponsor) = await _eventService.GetEventSpeakerById(events.Id.ToString());
				ViewBag.EventSponsor = hasSponsor;
				ViewBag.EventSpeaker = hasSpeaker;

				var sponsor = await _sponsorService.GetSponsorDetails();
				ViewBag.Sponsor = sponsor.ToList();

				var speak = await _speakerService.GetSpeakersAsync();
				ViewBag.Speak = speak.ToList();
				return View(updateEventDTO);
			}
			await _eventService.UpdateEventAsync(Id, updateEventDTO);

			return RedirectToAction(nameof(Index),"Event");
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
