using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class SpeakerDetail:BaseEntity
    {
		public string FullName { get; set; }
		public string Bio { get; set; }

		public Guid SpeakerId { get; set; }
		public Speaker Speaker { get; set; }

		public Guid LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
