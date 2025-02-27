using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class EventSpeaker :BaseEntity
	{
		public Guid EventId { get; set; }
		public Event Event { get; set; }
		public Guid SpeakerId { get; set; }
		public Speaker Speaker { get; set; }
	}
}
