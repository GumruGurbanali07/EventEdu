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
		private readonly ICategoryReadRepository _categoryReadRepository;
		private readonly ICategoryWriteRepository _categoryWriteRepository;

		public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository)
		{
			_categoryReadRepository = categoryReadRepository;
			_categoryWriteRepository = categoryWriteRepository;
		}

		public async Task<List<CategoryDetail>> GetCategoriesByLanguageAsync(string isoCode)
		{
			var categories = await _categoryReadRepository.GetAll()
				.Include(x => x.CategoryDetail)
				.ThenInclude(x => x.Language)
				.Where(z => z.CategoryDetail.Any(y => y.Language.IsoCode == isoCode))
				.SelectMany(c => c.CategoryDetail.Where(cd => cd.Language.IsoCode == isoCode))
				.ToListAsync();

			return categories;
		}

		public async Task AddCategoryWithLanguageAsync(string categoryName, Guid languageId)
		{
			var category = new Category
			{
				Id = Guid.NewGuid(),
				CategoryDetail = new List<CategoryDetail>
				{
					new CategoryDetail
					{
						Id = Guid.NewGuid(),
						CategoryName = categoryName,
						LanguageId = languageId
					}
				}
			};
			await _categoryWriteRepository.AddAsync(category);
			await _categoryWriteRepository.SaveChangeAsync();
		}
	}
}
