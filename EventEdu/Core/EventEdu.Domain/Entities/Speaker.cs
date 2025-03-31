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
	public class Speaker:BaseEntity
	{		
		public string ImageUrl { get; set; }

		[NotMapped]
		public IFormFile formFile { get; set; }
		public string Email { get; set; }
		public string FacebookLink { get; set; }
		public string TwitterLink { get; set; }
		public string InstagramLink { get; set; }
		public ICollection<SpeakerDetail> SpeakerDetails { get; set; }
		public ICollection<EventSpeaker> EventSpeakers { get; set; }

		
	}
}
