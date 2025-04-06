using EventEdu.Application.DTOs.Subscription;
using EventEdu.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventEdu.Webui.Controllers
{

	public class SubscriptionController : Controller
	{
		readonly private ISubscriptionService _subscriptionService;
		readonly private IEventService _eventService;

		public SubscriptionController(ISubscriptionService subscriptionService, IEventService eventService)
		{
			_subscriptionService = subscriptionService;
			_eventService = eventService;
		}

		public async Task<IActionResult> Create( Guid eventId)
		{
			var events = await _eventService.GetEventById(eventId.ToString());
			ViewBag.Event = events.Id;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(SubscribeDTO subscribeDTO, Guid eventId)
		{
			var events = await _eventService.GetEventById(eventId.ToString());
			ViewBag.Event = events.Id;
			try
			{
				await _subscriptionService.SubscribeToEventAsync(subscribeDTO, eventId);
				TempData["Success"] = "Uğurla qeydiyyatdan keçdiniz!";

				return Redirect("/");
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;  // Yalnız mesajı əlavə et

				return View(subscribeDTO);
			}

		}
	}
}
