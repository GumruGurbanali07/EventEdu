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
	public class Event:BaseEntity
	{	
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string ImageUrl { get; set; }

		[ForeignKey(nameof(Category))]		
		public Guid CategoryId { get; set; }
		public Category Category { get; set; }
		public ICollection<EventDetail> EventDetails { get; set; }
		public ICollection<SubsEvent> SubsEvents { get; set; } 
		public ICollection<EventSpeaker> EventSpeakers { get; set; }
		public ICollection<EventSponsor> EventSponsors { get; set; }
		public ICollection<FeedBack> FeedBacks { get; set; }

		[NotMapped]
		public IFormFile FormFile { get; set; }
		public string GetFormattedStartDate()
		{
			return StartDate.ToString("yyyy-MM-dd HH:mm");
		}

		public string GetFormattedEndDate()
		{
			return EndDate.ToString("yyyy-MM-dd HH:mm");
		}


	}
}
