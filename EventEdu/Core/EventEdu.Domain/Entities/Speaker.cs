using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class Speaker:BaseEntity
	{		
		public ICollection<SpeakerDetail> SpeakerDetails { get; set; }
		public ICollection<EventSpeaker> EventSpeakers { get; set; }

		public ICollection<ContactInfo> ContactInfos { get; set; }
	}
}
