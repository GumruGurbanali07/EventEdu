using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventEdu.Webui.ViewComponents
{
    public class SponsorViewComponent : ViewComponent
    {
        private readonly ISponsorService _sponsorService;
        private readonly AppDbContext _context;

        public SponsorViewComponent(ISponsorService sponsorService, AppDbContext context)
        {
            _sponsorService = sponsorService;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sponsor = await _context.Sponsors
    .Include(s => s.SponsorsDetail) // Eagerly load SponsorDetail
    .ToListAsync();
            var vm = new SponsIndexVM()
            {
                Sponsors = sponsor,
            };
            return View(vm);
        }
    }
}
