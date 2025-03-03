using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers;
[Area("Admin")]
public class FormLayoutsController : Controller
{
    public IActionResult Horizontal() => View();
    public IActionResult Vertical() => View();
}
