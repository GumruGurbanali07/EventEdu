using AutoMapper;
using EventEdu.Application.DTOs.Category;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SponsorController : Controller
    {
        private readonly ISponsorService _sponsorService;
        private readonly ILanguageService _languageService; 
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
		public SponsorController(ISponsorService sponsorService, AppDbContext context, IMapper mapper, ILanguageService languageService)
		{
			_sponsorService = sponsorService;
			_context = context;
			_mapper = mapper;
			_languageService = languageService;
		}

		public async Task<IActionResult> Index()
        {
            var sponsors = await _sponsorService.GetAllSponsorsByLanguageAsync("en");
            return View(sponsors);
        }

        [HttpGet]
        public async Task<IActionResult> AddSponsor()
        {
            var languages =await _languageService.GetLanguagesAsync();

            if (languages == null || !languages.Any())
            {
                ModelState.AddModelError("", "No languages found. Please add languages first.");
            }

			ViewBag.Languages = languages.Select(a => new SelectListItem()
			{

				Value = a.Id.ToString(),
				Text = a.Name

			});
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSponsor(CreateSponsorDTO addSponsorDTO)
        {

            if (!ModelState.IsValid)
            {
				var languages = await _languageService.GetLanguagesAsync();

				if (languages == null || !languages.Any())
				{
					ModelState.AddModelError("", "No languages found. Please add languages first.");
				}

				ViewBag.Languages = languages.Select(a => new SelectListItem()
				{

					Value = a.Id.ToString(),
					Text = a.Name

				});
				return View(addSponsorDTO);
            }


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
                var sponsor = await _sponsorService.GetSponsorById(id, "az-AZ");

                if (sponsor == null)
                {
                    return NotFound("Sponsor not found.");
                }

                var updateSponsorDTO = new CreateSponsorDTO
                {
                    SponsorName = sponsor.SponsorName,
                    SponsorDescription = sponsor.SponsorDescription,
                    Email = sponsor.Email,
                    PhoneNumber = sponsor.PhoneNumber,
                    Website = sponsor.Website,
                    Id = sponsor.Id,
                    ImagePath = sponsor.ImagePath,
                    LanguageId = sponsor.LanguageId
                };

                return View(updateSponsorDTO);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message); 
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSponsorAsync(Guid id, [FromForm] CreateSponsorDTO updateSponsorDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(updateSponsorDTO);
            }

            try
            {
                await _sponsorService.EditSponsor(id, updateSponsorDTO);
                TempData["Success"] = "Sponsor updated successfully!";
                return Redirect("Index");  
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);  
                return View(updateSponsorDTO);
            }
        }

    }
}
