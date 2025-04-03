using EventEdu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventEdu.Webui.ViewComponents
{
	public class CategoryViewComponent : ViewComponent
	{
		readonly private ICategoryService _categoryService;

		public CategoryViewComponent(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var category =await _categoryService.GetCategoriesAllAsync();
			return View(category);
		}
	}
}
