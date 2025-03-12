using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class Sponsor:BaseEntity
	{
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website {  get; set; }
		public string ImagePath { get; set; }

		public ICollection<EventSponsor>? EventSponsors { get; set; }
		public ICollection<EventSponsor> EventSponsors { get; set; }


        public ICollection<SponsorDetail>? SponsorsDetail { get; set; }
    }
}
