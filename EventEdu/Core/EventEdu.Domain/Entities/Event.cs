using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class Event:BaseEntity
	{	
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
		public ICollection<EventDetail> EventDetails { get; set; }
		public ICollection<SubsEvent> SubsEvents { get; set; } 
		public ICollection<EventSpeaker> EventSpeakers { get; set; }
		public ICollection<EventSponsor> EventSponsors { get; set; }
		public ICollection<FeedBack> FeedBacks { get; set; }





	}
}
