using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class EventSponsor
	{
		public Guid EventId { get; set; }
		public Event Event { get; set; }
		public Guid SponsorId { get; set; }
		public Sponsor Sponsor { get; set; }
	}
}
