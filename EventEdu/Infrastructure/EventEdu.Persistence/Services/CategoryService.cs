using AutoMapper;
using EventEdu.Application.DTOs;
using E=EventEdu.Application.DTOs;
using EventEdu.Application.DTOs.Category;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;


namespace EventEdu.Persistence.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IFileService _fileService;
		private readonly ICategoryReadRepository _categoryReadRepository;
		private readonly ICategoryWriteRepository _categoryWriteRepository;
		private readonly ICategoryDetailReadRepository _categoryDetailReadRepository;
		private readonly ICategoryDetailWriteRepository _categoryDetailWriteRepository;
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		private readonly IValidator<CreateCategoryDTO> _createCategoryValidator;
		private readonly IValidator<UpdateCategoryDTO> _updateCategoryValidator;
		private readonly IMapper _mapper;
		private readonly AppDbContext _context;
		public CategoryService(ICategoryReadRepository categoryReadRepository, ICategoryWriteRepository categoryWriteRepository, AppDbContext context, ICategoryDetailReadRepository categoryDetailReadRepository, ICategoryDetailWriteRepository categoryDetailWriteRepository, ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IMapper mapper, IValidator<CreateCategoryDTO> createCategoryValidator, IValidator<UpdateCategoryDTO> updateCategoryValidator, IFileService fileService)
		{
			_categoryReadRepository = categoryReadRepository;
			_categoryWriteRepository = categoryWriteRepository;
			_context = context;
			_categoryDetailReadRepository = categoryDetailReadRepository;
			_categoryDetailWriteRepository = categoryDetailWriteRepository;
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
			_mapper = mapper;
			_createCategoryValidator = createCategoryValidator;
			_updateCategoryValidator = updateCategoryValidator;
			_fileService = fileService;
		}


		public async Task AddCategoryWithLanguageAsync(CreateCategoryDTO createCategoryDTO)
		{
		
			var validationResult = await _createCategoryValidator.ValidateAsync(createCategoryDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			bool isCategoryExist = await _categoryDetailReadRepository.GetAll().AnyAsync(x => x.CategoryName == createCategoryDTO.CategoryName && x.LanguageId == createCategoryDTO.LanguageId);
			if (isCategoryExist)
			{
				throw new BadRequestException("This category already exists for the selected language.");
			}


			var language = await _languageReadRepository.GetByIdAsync(createCategoryDTO.LanguageId.ToString());
			if (language == null)
			{
				throw new NotFoundException("Selected language not found.");
			}

			
			var category = _mapper.Map<Category>(createCategoryDTO);
			
			category.Id = Guid.NewGuid();
			category.CreatedDate = DateTime.UtcNow;
			category.UpdatedDate = DateTime.UtcNow;
			

			

		

			await _categoryWriteRepository.AddAsync(category);
			await _categoryWriteRepository.SaveChangeAsync();

			var newFile = await _fileService.UploadAsync(createCategoryDTO.FormFile);

			var categoryDetail = new CategoryDetail
			{
				Id = Guid.NewGuid(),
				CategoryName = createCategoryDTO.CategoryName,
				CategoryId = category.Id,
				LanguageId = createCategoryDTO.LanguageId,
				ImagePath=newFile,
				CreatedDate = DateTime.UtcNow,
				UpdatedDate = DateTime.UtcNow,
			};

			//_context.CategoryDetails.Add(categoryDetail);
			//await _context.SaveChangesAsync();
			await _categoryDetailWriteRepository.AddAsync(categoryDetail);
			await _categoryDetailWriteRepository.SaveChangeAsync();
		}

		public async Task<(List<Category>, List<CategoryDetail>)> GetCategoriesByLanguageAsync(string isoCode)
		{
			// Get the language by ISO code
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				language = await _languageReadRepository.GetAll().FirstOrDefaultAsync(); // Default language
			}

			// Call GetAll() outside the LINQ query and store the result
			var categoryDetail =await _categoryDetailReadRepository.GetAll().Where(a=>a.LanguageId==language.Id).ToListAsync();

			// Use the stored query in the LINQ query
			var categories = await _categoryReadRepository.GetAll()

				
				.ToListAsync();





			return (categories, categoryDetail);


		}


		public async Task UpdateCategoryAsync(Guid categoryId, UpdateCategoryDTO updateCategoryDTO)
		{
			var validationResult = await _updateCategoryValidator.ValidateAsync(updateCategoryDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}
			var categoryDetail = await _categoryDetailReadRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Id == categoryId);
			if (categoryDetail == null)
			{
				throw new NotFoundException("Category not found.");
			}
			bool isCategoryExist = await _context.CategoryDetails
				.AnyAsync(x => x.CategoryName == updateCategoryDTO.CategoryName && x.LanguageId == updateCategoryDTO.LanguageId
				&& x.Id != categoryId);

			if (isCategoryExist)
			{
				throw new BadRequestException("This category name already exists for the selected language.");
			}

			//categoryDetail.CategoryName = updateCategoryDTO.CategoryName;
			//categoryDetail.LanguageId = updateCategoryDTO.LanguageId;
			//categoryDetail.UpdatedDate = DateTime.UtcNow;

			if (updateCategoryDTO.FormFile != null)
			{
				_fileService.Delete(categoryDetail.ImagePath);
				var newFile = await _fileService.UploadAsync(updateCategoryDTO.FormFile);
				categoryDetail.ImagePath = newFile;

			}
			_mapper.Map(updateCategoryDTO, categoryDetail);
			categoryDetail.UpdatedDate = DateTime.UtcNow;
		

			_categoryDetailWriteRepository.Update(categoryDetail);
			await _categoryDetailWriteRepository.SaveChangeAsync();
		}

		public async Task SoftDeleteCategoryAsync(Guid categoryId)
		{
			var categories = await _categoryReadRepository.GetByIdAsync(categoryId.ToString());
			if (categories == null)
			{
				throw new NotFoundException("Category not found.");
			}
			categories.SoftDelete();
			_categoryWriteRepository.Update(categories);

			var categoryDetails = await _categoryDetailReadRepository.GetAll()
				.Where(x => x.CategoryId == categoryId)
				.ToListAsync();

			foreach (var detail in categoryDetails)
			{
				detail.SoftDelete();
				_categoryDetailWriteRepository.Update(detail);

			}
			await _categoryWriteRepository.SaveChangeAsync();
		}

		public async Task RestoreCategoryAsync(Guid categoryId)
		{
			var categories = await _categoryReadRepository.GetByIdAsync(categoryId.ToString());
			if (categories == null)
			{
				throw new NotFoundException("Category not found.");
			}
			categories.Restore();
			_categoryWriteRepository.Update(categories);


			var categoryDetails = await _categoryDetailReadRepository.GetAll()
				.Where(x => x.CategoryId == categoryId)
				.ToListAsync();

			foreach (var detail in categoryDetails)
			{
				detail.Restore();
				_categoryDetailWriteRepository.Update(detail);

			}
			await _categoryWriteRepository.SaveChangeAsync();

		}

		public async Task<GetCategoryDTO?> GetCategoryByIdAndLanguageAsync(Guid categoryId, string isoCode)
		{
			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				language = await _languageReadRepository.GetAll().FirstOrDefaultAsync();
			}
			var categoryDetailsQuery = _categoryDetailReadRepository.GetAll();

			var category = await _categoryReadRepository.GetAll()
				.Where(c => c.Id == categoryId)
				.Select(c => new GetCategoryDTO
				{
					Id = c.Id,
					CategoryName = categoryDetailsQuery
						.Where(cd => cd.CategoryId == categoryId && cd.LanguageId == language.Id)
						.Select(cd => cd.CategoryName)
						.FirstOrDefault(),
					IsoCode = language.IsoCode,
				
				})
				.FirstOrDefaultAsync();

			if (category == null)
			{
				throw new NotFoundException("Category not found.");
			}

			return category;
		}

		public async Task<List<CategoryDetail>> GetCategoriesAllAsync()
		{
			var category = await _categoryDetailReadRepository.GetAll().ToListAsync();

			return category;
		}

		public async Task<CategoryDetail> GetCategoryById(Guid id)
		 => await _categoryDetailReadRepository.GetByIdAsync(id.ToString());
	}

}