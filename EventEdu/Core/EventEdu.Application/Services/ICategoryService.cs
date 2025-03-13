using EventEdu.Application.DTOs.Category;

namespace EventEdu.Application.Services
{
    public interface ICategoryService
    {
		//Task<List<CategoryDetail>> GetCategoriesByLanguageAsync(string isoCode);
		Task AddCategoryWithLanguageAsync(CreateCategoryDTO createCategoryDTO);
	    Task<List<GetCategoryDTO>> GetCategoriesByLanguageAsync(string isoCode);
		Task<GetCategoryDTO?> GetCategoryByIdAndLanguageAsync(Guid categoryId, string isoCode);
		Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryDTO updateCategoryDTO);
		Task SoftDeleteCategoryAsync(Guid categoryId);
		Task RestoreCategoryAsync(Guid categoryId);

	}
}
