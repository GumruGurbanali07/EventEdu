using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface ICategoryService
    {
		Task<bool> AddCategoryAsync(Category category);
		Task<bool> AddCategoryDetailAsync(CategoryDetail categoryDetail);
		Task<Category> GetCategoryByIdAsync(Guid categoryId);
		Task<IEnumerable<CategoryDetail>> GetCategoryDetailsByLanguageAsync(Guid categoryId, Guid languageId);
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<IEnumerable<CategoryDetail>> GetAllCategoryDetailsAsync();
		Task<bool> UpdateCategoryAsync(Category category);
		Task<bool> UpdateCategoryDetailAsync(CategoryDetail categoryDetail);
		Task<bool> DeleteCategoryAsync(Guid categoryId);
		Task<bool> DeleteCategoryDetailAsync(Guid categoryDetailId);
	}
}
