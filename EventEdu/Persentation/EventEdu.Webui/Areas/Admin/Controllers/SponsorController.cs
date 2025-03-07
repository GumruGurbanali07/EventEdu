using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SponsorController : Controller
    {
        private readonly ISponsorService _sponsorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;
        public SponsorController(ISponsorService sponsorService, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _sponsorService = sponsorService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sponsors = await _sponsorService.GetAllSponsorsByLanguageAsync("en");
            return View(sponsors);
        }

        [HttpGet]
        public IActionResult AddSponsor()
        {
            var languages = _context.Languages.ToList();

            if (languages == null || !languages.Any())
            {
                ModelState.AddModelError("", "No languages found. Please add languages first.");
            }

            ViewBag.Languages = languages; // Pass languages to the view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSponsor(CreateSponsorDTO addSponsorDTO)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View(addSponsorDTO);
            //}

            try
            {
                await _sponsorService.AddSponsor(addSponsorDTO);

                return RedirectToAction("Index", "Sponsor"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(addSponsorDTO); 
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSponsorAsync(Guid id)
        {
            var sponsor = await _sponsorService.GetSponsorById(id, "en"); 
            if (sponsor == null)
            {
                return NotFound();
            }
            return View(sponsor);
        }

        [HttpPost]
        public async Task<IActionResult> EditSponsorAsync(Guid id, CreateSponsorDTO updateSponsorDTO)
        {
            if (updateSponsorDTO == null)
            {
                return BadRequest("Sponsor data is required.");
            }

            try
            {
                var updatedSponsor = await _sponsorService.EditSponsor(id, updateSponsorDTO);
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(updateSponsorDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSponsor(Guid Id)
        {
            try
            {
                await _sponsorService.DeleteSponsor(Id);
                return Ok(new { message = "Sponsor soft deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
