//using EventEdu.Application.Services;
//using EventEdu.Application.ViewModel;
//using Microsoft.AspNetCore.Localization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewComponents;

//namespace EventEdu.Webui.ViewComponents
//{
//	public class LanguageSelectorViewComponent : ViewComponent
//	{
//		private readonly ILanguageService _languageService;

//		public LanguageSelectorViewComponent(ILanguageService languageService)
//		{
//			_languageService = languageService;
//		}
//		public async Task<ViewViewComponentResult> InvokeAsync()
//		{
//			var languages = await _languageService.GetLanguagesAsync();
//			var culture = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
//			var isoCode = culture?.Substring(culture.LastIndexOf("=") + 1) ?? "en-Us";
//			var selectedLanguage = await _languageService.GetLanguageAsync(isoCode);
//			var currentLanguageViewModel = new CurrenLanguageViewModel
//			{
//				Languages = languages,
//				SelectedLanguage = selectedLanguage
//			};
//			return View(currentLanguageViewModel);
//		}
//	}
//}
