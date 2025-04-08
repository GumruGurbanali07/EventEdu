using EventEdu.Application.DTOs.Event;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventEdu.Webui.Controllers;


public class EventController : Controller
{
	private readonly IEventService _eventService;
	private readonly ICategoryService _categoryService;
	private readonly ILanguageService _languageService;
	private readonly ISpeakerService _speakerService;
	private readonly ISponsorService _sponsorService;
	private readonly IHttpContextAccessor _contextAccessor;

	public EventController(IEventService eventService, ISpeakerService speakerService, ISponsorService sponsorService, ICategoryService categoryService, ILanguageService languageService, IHttpContextAccessor contextAccessor)
	{
		_eventService = eventService;
		_speakerService = speakerService;
		_sponsorService = sponsorService;
		_categoryService = categoryService;
		_languageService = languageService;
		_contextAccessor = contextAccessor;
	}




	public async Task<IActionResult> EventCategory(string categoryName)
	{


		var language = _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault();
		string isoCode = language?.Split(',').FirstOrDefault(); // ISO kodu götür

		var languages = await _languageService.GetLanguageAsync(isoCode);
		// Verileri al
		var events = await _eventService.GetEventCategoryAsync(categoryName, languages.IsoCode);

		// Eğer events null ise, NotFound döndür

		var eventSponsor = new List<SponsorDetail>();
		// Event speaker detaylarını hazırlama
		var eventSpeakerDetails = new List<SpeakerDetail>();
		foreach (var eve in events)
		{
			var speaker = await _speakerService.GetSpeakersEventByIdAsync(eve.Id.ToString());
			var sponsor = await _sponsorService.GetSponsorsById(eve.Id.ToString());
			eventSponsor = sponsor;
			eventSpeakerDetails = speaker;
		}

		// ViewModel oluşturma
		var vm = new EventIndex()
		{
			Events = events,
			SpeakerDetail = eventSpeakerDetails,
			SponsorDetail = eventSponsor
		};

		// Modeli View'a gönder
		return View(vm);
	}

	public async Task<IActionResult> EventDetail(Guid id, string isoCode)
	{
		var events = await _eventService.GetEventByIdAndLanguageAsync(id, isoCode);


		var speak = await _speakerService.GetSpeakersDByIdAsync(events.Id.ToString());


		if (events == null)
		{
			return NotFound();
		}

		var es = new EventSpeakerVM()
		{
			GetEventDTO = events,
			Speaker = speak.Item1,
			SpeakerDetail = speak.Item2,
		};


		return View(es);
	}

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Startup()
    {
        var events = await _eventService.GetEventAll();
        var startupEvents = events 
     .Where(e => e.Category != null
              && e.Category.CategoryDetail.FirstOrDefault().CategoryName == "Startup"
              && e.CategoryId == e.Category.Id)
     .ToList();


        return View(startupEvents);
    }



    public async Task<IActionResult> Business()
    {
        var events = await _eventService.GetEventAll();
        var businessEvents = events
              .Where(e => e.Category?.CategoryDetail?.FirstOrDefault()?.CategoryName == "Business")
            .ToList();




        return View(businessEvents);
    }

    public async Task<IActionResult> Science()
    {
        var events = await _eventService.GetEventAll();
        var scienceEvents = events
             .Where(e => e.Category?.CategoryDetail?.FirstOrDefault()?.CategoryName == "Science")
            .ToList();




        return View(scienceEvents);
    }

    public async Task<IActionResult> Technology()
    {
        var events = await _eventService.GetEventAll();
        var technologyEvents = events
              .Where(e => e.Category?.CategoryDetail?.FirstOrDefault()?.CategoryName == "Technology")
            .ToList();




        return View(technologyEvents);
    }

    public async Task<IActionResult> Education()
    {
        var events = await _eventService.GetEventAll();
        var educationEvents = events
              .Where(e => e.Category?.CategoryDetail?.FirstOrDefault()?.CategoryName == "Education")
            .ToList();




        return View(educationEvents);
    }


    public async Task<IActionResult> Search(string search)
    {
        var events = await _eventService.SearchEvents(search);

        return PartialView("_SearchPartial", events ?? new List<GetEventDTO>());
    }

}









