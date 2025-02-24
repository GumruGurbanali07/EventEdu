using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class Media:BaseEntity
	{
		public string FilePath { get; set; }
		public DateTime UploadedDate { get; set; }

		public Guid SponsorId { get; set; }
		public Sponsor Sponsor { get; set; }

		public Guid HeroSectionId { get; set; }
		public HeroSection HeroSection { get; set; }
	}
}
