using EventEdu.Application.DTOs.User;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventEdu.Webui.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(IUserService userService,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] UserRegisterDTO registerDTO)
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
                    return RedirectToAction("Login");
                }

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Login()

        {
            if(Request.Cookies.TryGetValue("RememberMeCredentials", out string rememberMeValue))
            {
                var values = rememberMeValue.Split('|');
                if(values.Length == 3)
                {
                    ViewBag.RememberMeEmail = values[0];
                    ViewBag.RememberMePassword = values[1];
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Dashboards");
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserLoginDTO userLoginDTO)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    return View(userLoginDTO);
                }
                if (userLoginDTO == null || string.IsNullOrEmpty(userLoginDTO.Email))
                {
                    return BadRequest("Username is required.");
                }

                var existUser = await _userManager.FindByEmailAsync(userLoginDTO.Email);

                if (existUser == null)
                {
                    ModelState.AddModelError("", "Sorry, your username or password was incorrect.");
                    return View(userLoginDTO);
                }
                var result = await _signInManager.PasswordSignInAsync(existUser, userLoginDTO.Password, false, true);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Sorry, your username or password was incorrect.");
                    return View(userLoginDTO);
                }

                HttpContext.Session.SetString("RememberMeEmail", userLoginDTO.Email);
                HttpContext.Session.SetString("RememberMePassword", userLoginDTO.Password);
                if (userLoginDTO.RememberMe)
                {
                    Response.Cookies.Append("RememberMeCredentials",
                        $"{userLoginDTO.Email}|" +
                        $"{userLoginDTO.Password}|", new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddYears(1),
                            Secure = true,
                            SameSite = SameSiteMode.None,
                        });
                }
                return RedirectToAction("Index", "Dashboards");
            }
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(ForgotPasswordDTO verifyEmailDTO)
        {
            if (ModelState.IsValid)
            {
                return View(verifyEmailDTO);
            }
            else
            {
                return RedirectToAction("ChangePassword", "User", new { userEmail = verifyEmailDTO.Email });
            }
            //return View(verifyEmailDTO);
        }

        [HttpGet]
        public IActionResult ChangePassword(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("VerifyEmail");
            }
            return View(new ForgotPasswordDTO { Email = userEmail });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ForgotPasswordDTO userChangePasswordDTO)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(userChangePasswordDTO.Email);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {

                        result = await _userManager.AddPasswordAsync(user, userChangePasswordDTO.NewPassword);
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userChangePasswordDTO);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(userChangePasswordDTO);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. Try again!");
                return View(userChangePasswordDTO);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Response.Cookies.Delete("RememberMeCredentials");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }









        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));

        //    return Content("Successed!");
        //}
    }
}
