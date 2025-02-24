using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
   public class EventDetail:BaseEntity
    {
		public string Title { get; set; }
		public string Description { get; set; }

		public Guid EventId { get; set; }
		public Event Event { get; set; }

		public Guid LanguageId { get; set; }
		public Language Language { get; set; }

	}
}
