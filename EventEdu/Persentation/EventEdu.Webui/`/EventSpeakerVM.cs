using EventEdu.Application.DTOs.Event;
using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels
{
	public class EventSpeakerVM
	{
		public List<Speaker> Speaker { get; set; }

		public List<SpeakerDetail> SpeakerDetail { get; set; }

		public GetEventDTO GetEventDTO {  get; set; }	
	}
}
