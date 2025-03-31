using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Speaker
{
    public class UpdateSpeakerDTO
    {
		public string FullName { get; set; }
		public string Bio { get; set; }
		public Guid LanguageId { get; set; }
		public string ImageUrl { get; set; }
		public List<SelectListItem> Language { get; set; }
		[NotMapped]
		public IFormFile FormFile { get; set; }
		public string Email { get; set; }
		public string FacebookLink { get; set; }
		public string TwitterLink { get; set; }
		public string InstagramLink { get; set; }
	}
}
