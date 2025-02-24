using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class HeroSectionDetail:BaseEntity
    {
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid HeroSectionId { get; set; }
		public HeroSection HeroSection { get; set; }

		public Guid LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
