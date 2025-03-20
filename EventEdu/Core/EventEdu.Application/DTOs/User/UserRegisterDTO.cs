using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.User
{
    public class UserRegisterDTO
    {
		public string Firstname { get; set; }
		public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
		public string Password { get; set; }
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
		
	}
}
