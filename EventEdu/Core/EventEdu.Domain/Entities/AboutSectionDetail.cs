using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{

    public class AboutSectionDetail:BaseEntity
    {
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Features { get; set; }

		public Guid AboutSectionId { get; set; }
		public AboutSection AboutSection { get; set; }

		public Guid LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
