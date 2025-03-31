using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EventEdu.Webui.Areas.Admin.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    [Area("Admin")]
    public class SpeakerController : Controller
    {
        private readonly ISpeakerService _speakerService;
        private readonly ILanguageService _languageService;


		public SpeakerController(ISpeakerService speakerService, ILanguageService languageService)
		{
			_speakerService = speakerService;
			_languageService = languageService;
		}


		public async Task<IActionResult> Index()
        {
            var speaker = await _speakerService.GetSpeakersAllAsync();

            return View(speaker);
        }
        //[HttpPost("AddSpeakerWithLanguage")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
			var getLanguage = await _languageService.GetLanguagesAsync();
			ViewBag.Language = getLanguage.Select(a => new SelectListItem()
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> Create(CreateSpeakerDTO model)
        {
            if (!ModelState.IsValid)
            {
				var getLanguage = await _languageService.GetLanguagesAsync();
				ViewBag.Language = getLanguage.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id.ToString()
				});

				return View(model);
            }
            await _speakerService.AddSpeakerWithLanguageAsync(model);

            return Redirect(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var speaker = await _speakerService.GetSpeakersByIdAsync(id);
			var getLanguage = await _languageService.GetLanguagesAsync();
			ViewBag.Language = getLanguage.Select(a => new SelectListItem()
			{
				Text = a.Name,
				Value = a.Id.ToString()
			});
			var us = new UpdateSpeakerDTO()
            {
                FullName = speaker.SpeakerDetail.FullName,
                Bio = speaker.SpeakerDetail.Bio,
                Email = speaker.Speaker.Email,
                FacebookLink = speaker.Speaker.FacebookLink,
                InstagramLink = speaker.Speaker.InstagramLink,
                TwitterLink = speaker.Speaker.TwitterLink,
                ImageUrl = speaker.Speaker.ImageUrl,
                LanguageId = speaker.SpeakerDetail.LanguageId,

            };

            return View(us);
        }
		[HttpPost]
		public async Task<IActionResult> Edit(string id, UpdateSpeakerDTO model )
        {
            if (!ModelState.IsValid)
            {
                var getLanguage = await _languageService.GetLanguagesAsync();
                ViewBag.Language = getLanguage.Select(a => new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });
                return View(model);

            }
            await _speakerService.UpdateSpeakerAsync(Guid.Parse(id), model);

            return Redirect(nameof(Index));
        }

        [HttpPost]
		public async Task<IActionResult> Delete (string id)
        {
			await _speakerService.SoftDeleteSpeakerAsync(Guid.Parse(id));
			return Redirect(nameof(Index));
		}
		//[HttpPut("restore/{speakerId}")]
		[HttpPost]
        public async Task<IActionResult> RestoreSpeaker(Guid speakerId)
        {
                await _speakerService.RestoreSpeakerAsync(speakerId);
            return Redirect(nameof(Index));
           
        }

    }
}
