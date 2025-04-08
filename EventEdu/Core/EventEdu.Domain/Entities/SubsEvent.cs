using EventEdu.Domain.Entities.Common;

namespace EventEdu.Domain.Entities
{
	public class SubsEvent : BaseEntity
	{
		public Guid EventId { get; set; }  
		public Event Event { get; set; }  
		
		public Guid SubscriptionId { get; set; }  
		public Subscription Subscription { get; set; }
	}
}
