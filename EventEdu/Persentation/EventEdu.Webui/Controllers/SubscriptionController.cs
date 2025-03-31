using EventEdu.Application.DTOs.Subscription;
using EventEdu.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{

	public class SubscriptionController : Controller
	{
		readonly private ISubscriptionService _subscriptionService;

		public SubscriptionController(ISubscriptionService subscriptionService)
		{
			_subscriptionService = subscriptionService;
		}

		public IActionResult Create()
			=> View();

		[HttpPost]
		public async Task<IActionResult> Create(SubscribeDTO subscribeDTO, Guid eventId)
		{
			try
			{
				await _subscriptionService.SubscribeToEventAsync(subscribeDTO, eventId);
				TempData["Success"] = "Uğurla qeydiyyatdan keçdiniz!";

				return Redirect("/");
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"Xəta baş verdi: {ex.Message}";
				return Redirect(nameof(Create));
			}

		}
	}
}
