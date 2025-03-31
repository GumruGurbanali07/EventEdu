using EventEdu.Application.Services;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.ViewComponents
{
	public class NavbarViewComponent : ViewComponent
	{
		readonly private ILanguageService _languageService;
		readonly private IHttpContextAccessor _contextAccessor;
		readonly private ICategoryService _categoryService;

		public NavbarViewComponent(ICategoryService categoryService, IHttpContextAccessor contextAccessor, ILanguageService languageService)
		{
			_categoryService = categoryService;
			_contextAccessor = contextAccessor;
			_languageService = languageService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{

			var language = _contextAccessor.HttpContext.Request.Headers["accept-language"].FirstOrDefault();
			string isoCode = language?.Split(',').FirstOrDefault();
			var category = await _categoryService.GetCategoriesByLanguageAsync(isoCode);
			var languages = await _languageService.GetLanguagesAsync();
			var vm = new CategoryLanguageVM()
			{
				CategoryDetails = category.Item2,
				Languages = languages
			};
			return View(vm);
		}
	}
}
