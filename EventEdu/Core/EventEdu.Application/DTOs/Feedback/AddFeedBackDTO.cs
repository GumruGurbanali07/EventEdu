using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Feedback
{
    public class AddFeedBackDTO
    {
		public double TotalRating { get; set; }

		public int Rating { get; set; }	
		public string Comment { get; set; }
		public Guid EventId { get; set; }
	
	}
}
