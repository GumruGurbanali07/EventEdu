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

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDTO registerDTO)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var result = await _userService.RegisterAsync(registerDTO);
					if (result.Succeeded)
					{
						return RedirectToAction("Login");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message); 
				}
			}

			return View(registerDTO);
		}

		
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
		{
			if (ModelState.IsValid)
			{
				var result = await _userService.LoginAsync(userLoginDTO);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError(string.Empty, "İstifadəçi adı və ya parol səhvdir.");
			}

			return View();
		}

		
		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _userService.LogOutAsync();
			return RedirectToAction("Login");
		}
	}
}
