using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Webui.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers;

public class LocalizerController : Controller
{
	public IActionResult ChangeCulture(string culture)
	{
		Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
			   CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
			   new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });
		return Redirect(Request.GetReturnUrl());
	}
}