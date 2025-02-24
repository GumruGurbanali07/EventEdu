using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class FeedBackDetail:BaseEntity
    {
		public string? Comment { get; set; }

		public Guid FeedBackId { get; set; }
		public FeedBack FeedBack { get; set; }

		public Guid LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
