using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Feedback
{
    public class AddFeedBackDTO
    {
		public double Rating { get; set; }
		public string Comment { get; set; }
		public Guid EventId { get; set; }
		public Guid LanguageId { get; set; }
		public Guid SubscriptionId { get; set; }
	}
}
