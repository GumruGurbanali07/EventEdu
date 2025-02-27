using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryWriteRepository _categoryWriteRepository;
		private readonly ICategoryReadRepository _categoryReadRepository;
		private readonly ICategoryDetailWriteRepository _categoryDetailWriteRepository;
		private readonly ICategoryDetailReadRepository _categoryDetailReadRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		public CategoryService(
			ICategoryWriteRepository categoryWriteRepository,
			ICategoryReadRepository categoryReadRepository,
			ICategoryDetailWriteRepository categoryDetailWriteRepository,
			ICategoryDetailReadRepository categoryDetailReadRepository,
			ILanguageReadRepository languageReadRepository,
			ILanguageWriteRepository languageWriteRepository)
		{
			_categoryWriteRepository = categoryWriteRepository;
			_categoryReadRepository = categoryReadRepository;
			_categoryDetailWriteRepository = categoryDetailWriteRepository;
			_categoryDetailReadRepository = categoryDetailReadRepository;
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
		}



		public async Task<bool> AddCategoryDetailAsync(CategoryDetailDTO categoryDetail)
		{
			if (categoryDetail == null)
			{
				throw new ArgumentNullException(nameof(categoryDetail));
			}

			var existingCategory =  _categoryReadRepository.GetAll().FirstOrDefault(x => x.CategoryDetail.Any(x => x.CategoryName == categoryDetail.CategoryName));
			if (existingCategory == null)
			{
				existingCategory = new Category();
				
				var categoryAdded = await _categoryWriteRepository.AddAsync(existingCategory);
				if (!categoryAdded)
					return false;
			}

			var languageExists = await _languageReadRepository.GetByIdAsync(categoryDetail.LanguageId.ToString());
			if (languageExists == null)
			{
				throw new Exception($"Language with ID {categoryDetail.LanguageId} not found.");
			}

			var newCategoryDetail = new CategoryDetail
			{
				CategoryId = existingCategory.Id, 
				CategoryName = categoryDetail.CategoryName, 
				LanguageId = categoryDetail.LanguageId
			};			
			await _categoryDetailWriteRepository.AddAsync(newCategoryDetail);
			await _categoryDetailWriteRepository.SaveChangeAsync();
			return true;
		}

		public async Task<Guid> GetLanguageIdByIsoCodeAsync(string isoCode)
		{
			var language = await _languageReadRepository.GetAll()
				.FirstOrDefaultAsync(x => x.IsoCode == isoCode);

			return language?.Id ?? Guid.Empty;
		}

		public async Task<List<CategoryDetail>> GetCategoryDetailsByLanguageAsync(Guid languageId)
		{
			return await _categoryDetailReadRepository.GetAll()
				.Where(x => x.LanguageId == languageId)
				.ToListAsync();
		}
		

		public Task<bool> DeleteCategoryDetailAsync(Guid categoryDetailId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<CategoryDetail>> GetAllCategoryDetailsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Category> GetCategoryByIdAsync(Guid categoryId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateCategoryDetailAsync(CategoryDetail categoryDetail)
		{
			throw new NotImplementedException();
		}
	}
}
