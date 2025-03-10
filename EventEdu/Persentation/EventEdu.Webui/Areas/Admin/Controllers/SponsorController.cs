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
        private readonly AppDbContext _context;
        public SponsorController(ISponsorService sponsorService, AppDbContext context)
        {
            _sponsorService = sponsorService;
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

            ViewBag.Languages = languages;
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

        [HttpPost]
        public async Task<IActionResult> DeleteSponsor(Guid id)
        {
            try
            {
                await _sponsorService.DeleteSponsor(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreSponsor(Guid id)
        {
            try
            {
                await _sponsorService.RestoreSponsor(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditSponsor(Guid id)
        {
            try
            {
                var sponsor = await _sponsorService.GetSponsorById(id,"en");

                if (sponsor == null)
                {
                    return NotFound();
                }

                //var updateSponsorDTO = new CreateSponsorDTO
                //{
                //    SponsorName = sponsor.SponsorName,
                //    SponsorDescription = sponsor.SponsorDescription,
                //    Email = sponsor.Email,
                //    PhoneNumber = sponsor.PhoneNumber,
                //    Website = sponsor.Website,
                //    //LanguageId = sponsorDTO.LanguageId
                //};


                return View(sponsor);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditSponsorAsync(Guid id, CreateSponsorDTO updateSponsorDTO)
        {
            try
            {
                var updatedSponsor = await _sponsorService.EditSponsor(id, updateSponsorDTO);
                TempData["Success"] = "Sponsor updated successfully!";
                return View(updateSponsorDTO);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(updateSponsorDTO);
            }
        }

    }
}
