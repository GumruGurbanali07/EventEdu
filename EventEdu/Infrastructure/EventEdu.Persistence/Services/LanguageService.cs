using AutoMapper;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Exceptions;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace EventEdu.Persistence.Services
{
	public class LanguageService : ILanguageService
	{
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		private readonly IValidator<CreateLanguageDTO> _createLanguageValidator;
		private readonly IValidator<UpdateLanguageDTO> _updateLanguageValidator;
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;
		public LanguageService(ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IMapper mapper, IValidator<CreateLanguageDTO> createLanguageValidator, IValidator<UpdateLanguageDTO> updateLanguageValidator, IFileService fileService, IHostingEnvironment environment)
		{
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
			_mapper = mapper;
			_createLanguageValidator = createLanguageValidator;
			_updateLanguageValidator = updateLanguageValidator;
			_fileService = fileService;
			_environment = environment;
		}

		public async Task<Language> CreateAsync(CreateLanguageDTO languageDTO)
		{
			//if (_context.Languages.Any(x => x.IsoCode == languageDTO.IsoCode))
			//{
			//	throw new InvalidOperationException("Bu ISO kodlu dil artıq mövcuddur.");
			//}
			var validationResult = await _createLanguageValidator.ValidateAsync(languageDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			if (await _languageReadRepository.GetByIsoCodeAsync(languageDTO.IsoCode) != null)
			{
				throw new BadRequestException("Bu ISO kodlu dil artıq mövcuddur.");

			}
            //var newLang = new Language
            //{
            //	IsoCode = languageDTO.IsoCode,
            //	ImagePath = languageDTO.ImagePath,
            //	Name = languageDTO.Name
            //};

            if (!languageDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!languageDTO.ImageFile.CheckFileSize(10))
            {
                throw new Exception("File size is too large. Maximum allowed size is 10MB.");
            }

            //string webRootPath = _environment.WebRootPath;
            //string imagePath = await _fileService.SaveFilesAsync(languageDTO.ImageFile, webRootPath, "client", "assets", "img", "languageMedias");


            var newLang = _mapper.Map<Language>(languageDTO);
            //languageDTO.ImagePath = imagePath;
            await _languageWriteRepository.AddAsync(newLang);
			await _languageWriteRepository.SaveChangeAsync();
			return newLang;
		}

		public async Task<LanguageGetDTO> GetLanguageAsync(string isoCode)
		{
			//var languages = _languageReadRepository.GetAll();

			//var language = languages.FirstOrDefault(x => x.IsoCode.ToLower() == isoCode.ToLower());

			//return _mapper.Map<LanguageViewModel>(language);

			var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);
			if (language == null)
			{
				throw new NotFoundException("Dil tapılmadı.");
			}
			//return new LanguageGetDTO
			//{
			//	Id = language.Id,
			//	Name = language.Name,
			//	IsoCode = language.IsoCode,
			//	ImagePath = language.ImagePath
			//};
			return _mapper.Map<LanguageGetDTO>(language);
		}

		public async Task<List<LanguageGetDTO>> GetLanguagesAsync()
		{
			var languages = await _languageReadRepository.GetAll().ToListAsync();
			//return languages.Select(x => new LanguageGetDTO
			//{
			//	Id = x.Id,
			//	Name = x.Name,
			//	IsoCode = x.IsoCode,
			//	ImagePath = x.ImagePath
			//}).ToList();
			return _mapper.Map<List<LanguageGetDTO>>(languages);
		}

		public async Task UpdateLanguageAsync(Guid id, UpdateLanguageDTO updateLanguageDTO)
		{
			var validationResult = await _updateLanguageValidator.ValidateAsync(updateLanguageDTO);
			if (!validationResult.IsValid)
			{
				throw new ValidationException(validationResult.Errors);
			}

			var language = await _languageReadRepository.GetByIdAsync(id);
			if (language == null)
			{
				throw new NotFoundException("Dil tapılmadı.");
			}

			//language.Name = updateLanguageDTO.Name;
			//language.IsoCode = updateLanguageDTO.IsoCode;
			//language.ImagePath = updateLanguageDTO.ImagePath;

			_mapper.Map(updateLanguageDTO, language);
			_languageWriteRepository.Update(language);
			await _languageWriteRepository.SaveChangeAsync();
		}

		public async Task SoftDeleteLanguageAsync(Guid languageId)
		{
			var language = await _languageReadRepository.GetByIdAsync(languageId);
			if (language == null)
			{
				throw new NotFoundException("Dil tapılmadı.");
			}
			language.SoftDelete();
			await _languageWriteRepository.SaveChangeAsync();
		}

		public async Task RestoreLanguageAsync(Guid languageId)
		{
			var language = await _languageReadRepository.GetByIdAsync(languageId);
			if (language == null)
			{
				throw new NotFoundException("Dil tapılmadı.");
			}
			language.Restore();
			await _languageWriteRepository.SaveChangeAsync();

		}




	}
}