using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels
{
	public class EventIndex
	{
		public List<Event> Events { get; set; }
		
		public List<SpeakerDetail> SpeakerDetail { get; set; }

		public List<SponsorDetail> SponsorDetail { get; set; }

	}
}
