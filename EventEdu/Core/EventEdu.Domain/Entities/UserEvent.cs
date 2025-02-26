using EventEdu.Domain.Entities.Common;
using EventEdu.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class UserEvent :BaseEntity
	{
		public string UserId { get; set; }
		public AppUser User { get; set; }
		public Guid EventId { get; set; }
		public Event Event { get; set; }
	}
}
