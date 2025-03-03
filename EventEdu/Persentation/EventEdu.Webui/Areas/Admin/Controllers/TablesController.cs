using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Areas.Admin.Controllers;
[Area("Admin")]
public class TablesController : Controller
{
    public IActionResult Basic() => View();
}
