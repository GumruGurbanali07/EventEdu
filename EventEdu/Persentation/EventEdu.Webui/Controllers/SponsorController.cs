using AutoMapper;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using EventEdu.Webui.ViewsModels.Sponsors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Controllers;

public class SponsorController : Controller
{
    private readonly ISponsorService _sponsorService;
    private readonly AppDbContext _context;

    public SponsorController(ISponsorService sponsorService, AppDbContext context)
    {
        _sponsorService = sponsorService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var sponsors = await _sponsorService.GetAllSponsorsByLanguageAsync("en");

        var sponsorViewModel = sponsors.Select(s => new SponsorVM
        {
            ImagePath = s.ImagePath,
            Name = s.SponsorName,
            Email = s.Email,
            Description = s.SponsorDescription,
        }).ToList();

        return View(sponsorViewModel);
    }

    public IActionResult Details(Guid id)
    {
        var sponsor = _sponsorService.GetSponsorById(id, "en");
        if (sponsor == null)
        {
            return NotFound();
        }
        return View(sponsor);
    }

}
