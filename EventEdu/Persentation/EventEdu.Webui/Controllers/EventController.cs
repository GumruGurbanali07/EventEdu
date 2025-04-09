using EventEdu.Application.DTOs.Event;
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

	//public IActionResult EventDetail()
	//{
	//	return View();
	//}

	public IActionResult Technology()
	{
		return View();
	}

	public IActionResult Startup()
	{
		return View();
	}

	public IActionResult Business()
	{
		return View();
	}

	public IActionResult Science()
	{
		return View();
	}

	public IActionResult Education()
	{
		return View();
	}
}










