using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels
{
	public class SponsIndexVM
	{
		public List<Sponsor> Sponsors { get; set; }	
		public SponsorDetail SponsorDetail { get; set; }
	}
}
