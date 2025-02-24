using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
   public class CategoryDetail:BaseEntity
    {
		public string CategoryName { get; set; }

		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
		public Guid LanguageId { get; set; }
		public Language Language { get; set; }

	}
}
