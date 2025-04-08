using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Feedback
{
    public class GetFeedbackDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string FullName { get; set; }
        public string EventName { get; set; }

        public double Rating { get; set; }
		public string Comment { get; set; }
		public double TotalRating { get; set; }	
		public bool IsDeleted { get; set; }
	}
}
