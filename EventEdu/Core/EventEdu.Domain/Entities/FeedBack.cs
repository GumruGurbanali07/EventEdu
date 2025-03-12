using EventEdu.Domain.Entities.Common;
using EventEdu.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class FeedBack : BaseEntity
	{
		public double Rating { get; set; } 
		public ICollection<FeedBackDetail> FeedBackDetails { get; set; } 

		public string UserId { get; set; }
		public AppUser User { get; set; } 
	}
}
