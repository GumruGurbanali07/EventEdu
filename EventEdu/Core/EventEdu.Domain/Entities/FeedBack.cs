using EventEdu.Domain.Entities.Common;

namespace EventEdu.Domain.Entities
{
	public class FeedBack : BaseEntity
	{
		public double Rating { get; set; } 
		public ICollection<FeedBackDetail> FeedBackDetails { get; set; }

		public Guid EventId { get; set; } 
		public Event Event { get; set; }

		public Guid SubsEventId { get; set; } 
		public SubsEvent SubsEvent { get; set; }
	}
}
