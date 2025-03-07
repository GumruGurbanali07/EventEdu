using EventEdu.Application.DTOs.User;
using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
    {
        private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}


		[HttpPost("RegisterUser")]
		public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var result = await _userService.RegisterAsync(registerDTO);

				if (result.Succeeded)
				{
					return Ok(new { message = "Qeydiyyat uğurla tamamlandı." });
				}

				return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}



		[HttpPost("LoginUser")]
		public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _userService.LoginAsync(userLoginDTO);
			if (result.Succeeded)
			{
				return Ok(new { message = "Giriş uğurludur." });
			}

			return BadRequest(new { error = "İstifadəçi adı və ya parol səhvdir." });
		}


		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _userService.LogOutAsync();
			return Ok(new { message = "Çıxış edildi." });
		}
	}
}
