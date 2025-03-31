using EventEdu.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	
   public class CategoryDetail:BaseEntity
    {
		public string ImagePath { get; set; }

		[NotMapped]
		public IFormFile FormFile { get; set; }
		public string CategoryName { get; set; }

		[ForeignKey(nameof(Category))]
		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
		public Guid LanguageId { get; set; }
		public Language Language { get; set; }

	}
}
