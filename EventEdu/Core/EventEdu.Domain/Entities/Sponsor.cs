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
		public string Website {  get; set; }

		public ICollection<Media> Medias { get; set; }
		public ICollection<ContactInfo> ContactInfos { get; set; }
		public ICollection<EventSponsor> EventSponsors { get; set; }


	}
}
