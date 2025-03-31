using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Event
{
    public class CreateEventDTO
    {
		public string Title { get; set; }
		public string Description { get; set; }
		public string? ImageUrl { get; set; }
		public Guid CategoryId { get; set; }
		
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		[NotMapped]
		public IFormFile FormFile { get; set; }
		public string FormattedStartDate => StartDate.ToString("yyyy-MM-dd HH:mm");
		public string FormattedEndDate => EndDate.ToString("yyyy-MM-dd HH:mm");
		public Guid LanguageId { get; set; }

		public List<SelectListItem>? Category { get; set; }
		public List<SelectListItem>? Language { get; set; }

		public List<Guid> SpeakerId { get; set; }	

		public List<Guid> SponsorId { get; set; }	
	}
}
