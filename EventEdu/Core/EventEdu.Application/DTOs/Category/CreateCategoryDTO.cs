using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Category
{
    public class CreateCategoryDTO
    {
		public string? ImagePath { get; set; }

		[NotMapped]
		public IFormFile FormFile { get; set; }
		public string CategoryName { get; set; }
		public ICollection<SelectListItem>? Language { get; set; }
		public Guid LanguageId { get; set; }


	}
}
