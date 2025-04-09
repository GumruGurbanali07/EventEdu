using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly AppDbContext _context;
        public SubscriptionController(AppDbContext context, ISubscriptionService subscriptionService)
        {
            _context = context;
            _subscriptionService = subscriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var subscriptions = await _context.Subscriptions
                .Include(s => s.SubsEvents)
                    .ThenInclude(se => se.Event)
                        .ThenInclude(e => e.EventDetails)
                .ToListAsync();

            return View(subscriptions);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string email, Guid eventId)
        {
            try
            {
                await _subscriptionService.UnsubscribeFromEventAsync(email, eventId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
