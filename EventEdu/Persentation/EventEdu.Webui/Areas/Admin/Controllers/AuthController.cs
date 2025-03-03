using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventEdu.Webui.Areas.Admin.Controllers;
[Area("Admin")]
public class AuthController : Controller
{
    public IActionResult ForgotPasswordBasic() => View();
    public IActionResult LoginBasic() => View();
    public IActionResult RegisterBasic() => View();
}
