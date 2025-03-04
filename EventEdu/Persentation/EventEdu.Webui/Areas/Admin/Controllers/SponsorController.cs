using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SponsorController : Controller
    {
        private readonly ISponsorService _sponsorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SponsorController(ISponsorService sponsorService, IWebHostEnvironment webHostEnvironment)
        {
            _sponsorService = sponsorService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddSponsor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSponsorAsync(CreateSponsorDTO sponsorDTO)
        {

            if (!ModelState.IsValid)
            {
                return View(sponsorDTO);
            }
            else
            {
                if (!sponsorDTO.ImageFile.CheckFileType("image"))
                {
                    ModelState.AddModelError("", "Invalid File");
                    return View(sponsorDTO);
                }
                if (!sponsorDTO.ImageFile.CheckFileSize(10))
                {
                    ModelState.AddModelError("", "Invalid File Size");
                    return View(sponsorDTO);
                }

                string uniqueFileName = await sponsorDTO.ImageFile.SaveFilesAsync(_webHostEnvironment.WebRootPath, "client", "assets", "img", "categoryIcons");

                CreateSponsorDTO newsponsorDTO = new CreateSponsorDTO
                {
                    ImageFile = sponsorDTO.ImageFile
                };
            }

            try
            {
                await _sponsorService.AddSponsorAsync(sponsorDTO);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(sponsorDTO);
            }
        }

        [HttpGet]
        public IActionResult EditSponsor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSponsor(Guid Id)
        {
            try
            {
                await _sponsorService.DeleteSponsor(Id);
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
