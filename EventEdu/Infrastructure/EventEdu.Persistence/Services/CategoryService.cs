using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
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

		public CategoryService(
			ICategoryWriteRepository categoryWriteRepository,
			ICategoryReadRepository categoryReadRepository,
			ICategoryDetailWriteRepository categoryDetailWriteRepository,
			ICategoryDetailReadRepository categoryDetailReadRepository)
		{
			_categoryWriteRepository = categoryWriteRepository;
			_categoryReadRepository = categoryReadRepository;
			_categoryDetailWriteRepository = categoryDetailWriteRepository;
			_categoryDetailReadRepository = categoryDetailReadRepository;
		}
		public Task<bool> AddCategoryAsync(Category category)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddCategoryDetailAsync(CategoryDetail categoryDetail)
		{
			throw new NotImplementedException();
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

		public Task<CategoryDetail> GetCategoryDetailByIdAsync(Guid categoryDetailId)
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
