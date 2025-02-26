using EventEdu.Application.DTOs.Category;
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
		
		Task<bool> AddCategoryDetailAsync(CategoryDetailDTO categoryDetail);
		Task<Category> GetCategoryByIdAsync(Guid categoryId);
		Task<List<CategoryDetail>> GetCategoryDetailsByLanguageAsync(Guid languageId);
		Task<IEnumerable<Category>> GetAllCategoriesAsync();
		Task<IEnumerable<CategoryDetail>> GetAllCategoryDetailsAsync();
		
		Task<bool> UpdateCategoryDetailAsync(CategoryDetail categoryDetail);
		
		Task<bool> DeleteCategoryDetailAsync(Guid categoryDetailId);
	}
}
