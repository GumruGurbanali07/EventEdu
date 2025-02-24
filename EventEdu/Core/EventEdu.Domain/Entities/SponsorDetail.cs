using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
   public class SponsorDetail:BaseEntity
    {
		public string SponsorName { get; set; }
		public Guid SponsorId { get; set; }
		public Sponsor Sponsor { get; set; }
		public Guid LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
