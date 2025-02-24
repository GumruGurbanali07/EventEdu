using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers;

public class LanguageController : Controller
{
    public IActionResult Change(string? lang)
    {
        if (!string.IsNullOrEmpty(lang))
        {
            HttpContext.Session.SetString("lang", lang);
        }

        return RedirectToAction("Index", "Home");
    }
}