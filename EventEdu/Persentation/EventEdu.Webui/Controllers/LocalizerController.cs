using EventEdu.Application.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace EventEdu.Webui.Controllers
{
    public class LocalizerController : Controller
    {
		private readonly ILanguageService _languageService;

		public LocalizerController(ILanguageService languageService)
		{
			_languageService = languageService;
		}

		public IActionResult ChangeCulture(string culture)
		{
			Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

			return Redirect(Request.Headers["Referer"].ToString());
		}

		public async Task<int> GetLanguageAsync()
		{
			var culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
			var isoCode = culture?.Substring(culture.LastIndexOf("=") + 1) ?? "en-Us";
			var selectedLanguage = await _languageService.GetLanguageAsync(isoCode);

			return selectedLanguage.Id;
		}
	}
}
