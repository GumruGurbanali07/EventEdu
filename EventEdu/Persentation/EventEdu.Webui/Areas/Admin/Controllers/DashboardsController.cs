using System.Diagnostics;
using EventEdu.Persistence.Context;
using EventEdu.Webui.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class DashboardsController : Controller
{
    private readonly AppDbContext _context;

    public DashboardsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var sponsors = _context.Sponsors
            .Where(s => !s.IsDeleted)
            .Include(s => s.SponsorsDetail)
            .GroupBy(s => s.SponsorsDetail.FirstOrDefault().SponsorName)
            .Select(g => g.First())
            .ToList();

        var dashboardVM = new DashboardsVM
        {
            Sponsors = sponsors
        };

        return View(new List<DashboardsVM> { dashboardVM });
    }
}
