using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers;
[Area("Admin")]
public class ExtendedUiController : Controller
{
    public IActionResult PerfectScrollbar() => View();
    public IActionResult TextDivider() => View();
}
