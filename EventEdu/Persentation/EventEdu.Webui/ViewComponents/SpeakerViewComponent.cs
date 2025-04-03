using EventEdu.Application.Services;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.ViewComponents
{
	public class SpeakerViewComponent : ViewComponent
	{
		readonly private ISpeakerService _speakerService;

		public SpeakerViewComponent(ISpeakerService speakerService)
		{
			_speakerService = speakerService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var speak = await _speakerService.GetSpeakersAllAsync();
			var vm = new SpeakerIndexVM()
			{
				Speakers = speak.Item1,
				SpeakerDetail = speak.Item2,
			};
			return View(vm);
		}
	}
}
