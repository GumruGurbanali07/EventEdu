using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class AboutSection:BaseEntity
    {
		
		public ICollection<AboutSectionDetail> AboutSectionDetails { get; set; }
		public ICollection<Media> Medias { get; set; }
		
	}
}
