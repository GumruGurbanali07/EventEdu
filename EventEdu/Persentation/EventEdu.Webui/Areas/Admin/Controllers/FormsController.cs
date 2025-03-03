using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers;
[Area("Admin")]
public class FormsController : Controller
{
    public IActionResult BasicInputs() => View();
    public IActionResult InputGroups() => View();
}
