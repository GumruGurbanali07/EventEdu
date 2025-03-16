using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Speaker
{
	public class CreateSpeakerDTO
	{
		public string FullName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Bio { get; set; }
		public Guid LanguageId { get; set; }
		//public Guid SpeakerId { get; set; }
		public string ImageUrl { get; set; }
		public string Email { get; set; }
		public string FacebookLink { get; set; }
		public string TwitterLink { get; set; }
		public string InstagramLink { get; set; }
	}
}
