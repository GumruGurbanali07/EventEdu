using EventEdu.Application.Services;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.ViewComponents
{
	public class AboutViewComponent : ViewComponent
	{
		readonly private IAboutSectionService _aboutSectionService;

		public AboutViewComponent(IAboutSectionService aboutSectionService)
		{
		_aboutSectionService = aboutSectionService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var aboutSection = await _aboutSectionService.GetAboutSectionAll();

			var vm = new AboutIndexVM()
			{
				AboutSection = aboutSection.Item1,
				AboutSectionDetail = aboutSection.Item2,
			};
			return View(vm);
		}
	}
}
