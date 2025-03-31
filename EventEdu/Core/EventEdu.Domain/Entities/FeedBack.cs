using EventEdu.Domain.Entities.Common;

namespace EventEdu.Domain.Entities
{
	public class FeedBack : BaseEntity
	{
		public double Rating { get; set; } 
		public ICollection<FeedBackDetail> FeedBackDetails { get; set; }

		public Guid EventId { get; set; } 
		public Event Event { get; set; }

		public Rating RatingEvenets { get; set; }	

		public Guid SubscriptionId { get; set; }
		public Subscription Subscription { get; set; }
	}
}
