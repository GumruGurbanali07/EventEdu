using EventEdu.Application.Services;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.ViewComponents
{
	public class SponsorViewComponent : ViewComponent
	{
		private readonly ISponsorService _sponsorService;

		public SponsorViewComponent(ISponsorService sponsorService)
		{
			_sponsorService = sponsorService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var sponsor = await _sponsorService.GetSponsorAll();
			var vm = new SponsIndexVM()
			{
				Sponsors = sponsor.Item1.ToList(),
				SponsorDetail = sponsor.Item2,
			};
			return View(vm);
		}
	}
}
