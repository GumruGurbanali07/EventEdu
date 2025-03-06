using EventEdu.Application.DTOs.User;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public async Task<IdentityResult> RegisterAsync(UserRegisterDTO registerDTO)
		{
			var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
			if (existingUser != null)
			{
				throw new Exception("Bu email ilə artıq istifadəçi mövcuddur.");

			}
			if (registerDTO.Password != registerDTO.ConfirmPassword)
			{
				throw new Exception("Parollar uyğun gəlmir.");

			}

			var user = new AppUser
			{
				UserName = registerDTO.Firstname + registerDTO.Lastname,
				Email = registerDTO.Email,
			};
			var result = await _userManager.CreateAsync(user, registerDTO.Password);
			return result;
		}

		public async Task<SignInResult> LoginAsync(UserLoginDTO userLoginDTO)
		{
			var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
			if (user == null)
			{
				return SignInResult.Failed;

			}
			var result = await _signInManager.PasswordSignInAsync(user, userLoginDTO.Password, userLoginDTO.RememberMe, false);
			
			if (!result.Succeeded)
			{
				return SignInResult.Failed;
			}
			return result;
		}

		public async Task LogOutAsync()
		{
			await _signInManager.SignOutAsync();
		}


	}
}
