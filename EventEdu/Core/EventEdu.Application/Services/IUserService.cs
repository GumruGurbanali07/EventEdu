using EventEdu.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IUserService
    {
		Task<IdentityResult> RegisterAsync(UserRegisterDTO registerDTO);
		Task<SignInResult> LoginAsync(UserLoginDTO userLoginDTO);
		Task LogOutAsync();

	}
}
