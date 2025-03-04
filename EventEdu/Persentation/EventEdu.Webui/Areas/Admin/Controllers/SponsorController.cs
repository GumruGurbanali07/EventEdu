using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SponsorController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddSponsor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSponsor(CreateSponsorDTO addSponsorDTO)
        {
        
            return View();
        }
        [HttpGet]
        public IActionResult EditSponsor()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteSponsor()
        {
            return View();
        }
    }
}
