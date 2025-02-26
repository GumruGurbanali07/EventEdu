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

		public async Task<bool> AddCategoryAsync(Category category)
		{
			if (category == null)
			{
				throw new ArgumentNullException(nameof(category));
			}
			var result = await _categoryWriteRepository.AddAsync(category);
			if (result)
			{
				await _categoryWriteRepository.SaveChangeAsync();
			}
			return result;
		}


		public async Task<bool> AddCategoryDetailAsync(CategoryDetail categoryDetail)
		{
			if (categoryDetail == null)
			{
				throw new ArgumentNullException(nameof(categoryDetail));
			}
			var existingCategoryDetail = _categoryDetailReadRepository.GetAll().FirstOrDefault(x => x.Id == categoryDetail.CategoryId && x.LanguageId == categoryDetail.LanguageId);
			if (existingCategoryDetail != null)
			{
				throw new Exception("This category already has a detail for the selected language.");
			}
			var lang = await _languageReadRepository.GetByIdAsync(categoryDetail.Id.ToString());
			if (lang == null)
			{
				throw new Exception("Language not found");
			}
			var result = await _categoryDetailWriteRepository.AddAsync(categoryDetail);
			if (result)
			{
				await _categoryDetailWriteRepository.SaveChangeAsync();
			}

			return result;
		}
		public async Task<IEnumerable<CategoryDetail>> GetCategoryDetailsByLanguageAsync(Guid categoryId, Guid languageId)
		{
			return await _categoryDetailReadRepository.GetAll()
				.Where(x => x.CategoryId == categoryId && x.LanguageId == languageId)
				.ToListAsync();
		}
		public Task<bool> DeleteCategoryAsync(Guid categoryId)
		{
			throw new NotImplementedException();
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



		public Task<bool> UpdateCategoryAsync(Category category)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateCategoryDetailAsync(CategoryDetail categoryDetail)
		{
			throw new NotImplementedException();
		}
	}
}
