using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class HeroSection:BaseEntity
    {
		public string Title { get; set; }
		public string Description { get; set; }
		public ICollection<Media> Medias { get; set; }
		public ICollection<HeroSectionDetail> HeroSectionDetails { get; set; }
		
	}
}
