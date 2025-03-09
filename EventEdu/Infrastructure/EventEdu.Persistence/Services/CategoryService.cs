using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
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
		private readonly AppDbContext _context;
		public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository, AppDbContext context)
		{
			_categoryReadRepository = categoryReadRepository;
			_categoryWriteRepository = categoryWriteRepository;
			_context = context;
		}

		//public async Task<List<CategoryDetail>> GetCategoriesByLanguageAsync(string isoCode)
		//{
		//	var categories = await _categoryReadRepository.GetAll()
		//		.Include(x => x.CategoryDetail)
		//		.ThenInclude(x => x.Language)
		//		.Where(z => z.CategoryDetail.Any(y => y.Language.IsoCode == isoCode))
		//		.SelectMany(c => c.CategoryDetail.Where(cd => cd.Language.IsoCode == isoCode))
		//		.ToListAsync();
		//	return categories;
		//}

		public async Task AddCategoryWithLanguageAsync(CreateCategoryDTO createCategoryDTO)
		{
			bool isCategoryExist = await _categoryReadRepository.GetAll().AnyAsync(x=>x.CategoryDetail.Any(x => x.CategoryName == createCategoryDTO.CategoryName && x.LanguageId == createCategoryDTO.LanguageId));
			if (isCategoryExist)
			{
				throw new Exception("This category already exists for the selected language.");

			}

			var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == createCategoryDTO.LanguageId);
			if (language == null)
			{
				throw new Exception("Selected language not found.");
			}

			var category = new Category
			{
				Id = Guid.NewGuid(),
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.UtcNow,
			};

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();


			var categoryDetail = new CategoryDetail
			{
				Id = Guid.NewGuid(),
				CategoryName = createCategoryDTO.CategoryName,
				CategoryId = category.Id,
				LanguageId = createCategoryDTO.LanguageId,
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.UtcNow,
			};

			_context.CategoryDetails.Add(categoryDetail);
			await _context.SaveChangesAsync();

		}
		public async Task<List<GetCategoryDTO>> GetCategoriesByLanguageAsync(string isoCode)
		{
			var language = await _context.Languages.FirstOrDefaultAsync(l => l.IsoCode == isoCode);

			if (language == null)
			{
				language = await _context.Languages.FirstAsync();
			}

			var categories = await _context.Categories
				.Where(c => _context.CategoryDetails
					.Any(cd => cd.CategoryId == c.Id && cd.LanguageId == language.Id))
				.Select(c => new GetCategoryDTO
				{
					Id = c.Id,
					CategoryName = _context.CategoryDetails
						.Where(cd => cd.CategoryId == c.Id && cd.LanguageId == language.Id)
						.Select(cd => cd.CategoryName)
						.FirstOrDefault(),
					IsoCode = language.IsoCode,
					ImagePath = language.ImagePath
				})
				.ToListAsync();

			return categories;
		}



		public async Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryDTO updateCategoryDTO)
		{
			var categoryDetail = await _context.CategoryDetails
				.FirstOrDefaultAsync(x => x.Id == categoryId);
			if (categoryDetail == null)
			{
				throw new Exception("Category not found");
			}
			bool isCategoryExist = await _context.CategoryDetails
				.AnyAsync(x => x.CategoryName == updateCategoryDTO.CategoryName && x.LanguageId == updateCategoryDTO.LanguageId
				&& x.Id != categoryId);

			if (isCategoryExist)
			{
				throw new Exception("This category name already exists for the selected language.");
			}

			categoryDetail.CategoryName = updateCategoryDTO.CategoryName;
			categoryDetail.LanguageId = updateCategoryDTO.LanguageId;
			categoryDetail.UpdatedDate = DateTime.UtcNow;
			await _context.SaveChangesAsync();
		}
		public async Task SoftDeleteCategoryAsync(Guid categoryId)
		{
			var categories = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
			if (categories == null)
			{
				throw new Exception("Category not found");
			}
			categories.SoftDelete();
			var categoryDetails = await _context.CategoryDetails.Where(x => x.CategoryId == categoryId).ToListAsync();
			foreach (var detail in categoryDetails)
			{
				detail.SoftDelete();
			}
			await _context.SaveChangesAsync();
		}
		public async Task RestoreCategoryAsync(Guid categoryId)
		{
			var categories = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
			if (categories == null)
			{
				throw new Exception("Category not found");
			}
			categories.Restore();
			var categoryDetails = await _context.CategoryDetails.Where(x => x.CategoryId == categoryId).ToListAsync();
			foreach(var detail in categoryDetails)
			{
				detail.Restore();
			}
			await _context.SaveChangesAsync();



		}

		public async Task<GetCategoryDTO?> GetCategoryByIdAndLanguageAsync(Guid categoryId, string isoCode)
		{
			var language = await _context.Languages.FirstOrDefaultAsync(l => l.IsoCode == isoCode);
			if (language == null)
			{
				language = await _context.Languages.FirstAsync(); // Default dili götür
			}

			var category = await _context.Categories
				.Where(c => c.Id == categoryId)
				.Select(c => new GetCategoryDTO
				{
					Id = c.Id,
					CategoryName = _context.CategoryDetails
						.Where(cd => cd.CategoryId == categoryId && cd.LanguageId == language.Id)
						.Select(cd => cd.CategoryName)
						.FirstOrDefault(),
					IsoCode = language.IsoCode,
					ImagePath = language.ImagePath
				})
				.FirstOrDefaultAsync();

			return category;
		}

	}

}