using EventEdu.Domain.Entities.Common;
using EventEdu.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
