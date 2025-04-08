using EventEdu.Domain.Entities.Common;
using System.Globalization;

namespace EventEdu.Domain.Entities
{
	public class FeedBack : BaseEntity
	{
		public  string FullName { get; set; }	
		public double TotalRating { get; set; }
		
		public int Rating { get; set; }	
		public string? Comment { get; set; }
	     
		public Guid EventId { get; set; }	
		public Event Event { get; set; }
	}
}
