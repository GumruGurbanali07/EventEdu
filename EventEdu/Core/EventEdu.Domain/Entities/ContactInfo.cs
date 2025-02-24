using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class ContactInfo:BaseEntity
    {
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string FacebookLink { get; set; }
		public string LinkedInLink { get; set; }
		public string InstagramLink { get; set; }

		public Guid SponsorId { get; set; }
		public Sponsor Sponsor { get; set; }

	
		public Guid SpeakerId { get; set; }
		public Speaker Speaker { get; set; }
	}
}
