using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.User
{
    public class UserLoginDTO
    {
		public string  Email { get; set; }
		public string Password { get; set; }
		public bool  RememberMe { get; set; }

	}
}
