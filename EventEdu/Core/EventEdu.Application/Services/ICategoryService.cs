using EventEdu.Application.DTOs;
using EventEdu.Application.DTOs.Category;
using EventEdu.Domain.Entities;

namespace EventEdu.Application.Services
{
	public interface ICategoryService
	{
		//Task<List<CategoryDetail>> GetCategoriesByLanguageAsync(string isoCode);
		Task AddCategoryWithLanguageAsync(CreateCategoryDTO createCategoryDTO);
		Task<List<CategoryDetail>> GetCategoriesAllAsync();
		Task<CategoryDetail> GetCategoryById(Guid id);
		Task<(List<Category> , List<CategoryDetail> )> GetCategoriesByLanguageAsync(string isoCode);
		Task<GetCategoryDTO?> GetCategoryByIdAndLanguageAsync(Guid categoryId, string isoCode);
		Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryDTO updateCategoryDTO);
		Task SoftDeleteCategoryAsync(Guid categoryId);
		Task RestoreCategoryAsync(Guid categoryId);

	}
}
