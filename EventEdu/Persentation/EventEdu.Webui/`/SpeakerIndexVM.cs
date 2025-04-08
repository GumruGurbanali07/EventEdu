using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels
{
	public class SpeakerIndexVM
	{
		public List<Speaker> Speakers { get; set; }

		public List<SpeakerDetail> SpeakerDetail { get; set; }
	}
}
