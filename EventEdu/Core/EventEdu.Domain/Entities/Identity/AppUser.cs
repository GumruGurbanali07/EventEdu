using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities.Identity
{
	public class AppUser:IdentityUser
	{
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string? ResetPassword { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenDate { get; set; }

		public ICollection<FeedBack>? FeedBacks { get; set; }
	}
}
