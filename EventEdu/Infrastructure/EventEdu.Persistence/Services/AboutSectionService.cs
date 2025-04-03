using AutoMapper;
using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
    public class AboutSectionService : IAboutSectionService
    {
        private readonly AppDbContext _context;
        private readonly IAboutSectionReadRepository _aboutReadRepository;
        private readonly IAboutSectionWriteRepository _aboutWriteRepository;
        private readonly IAboutSectionDetailReadRepository _aboutDetailReadRepository;
        private readonly IAboutSectionDetailWriteRepository _aboutDetailWriteRepository;
        private readonly ILanguageReadRepository _languageReadRepository;
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAboutSectionDTO> _createAboutSectionValidator;

        public AboutSectionService(AppDbContext context,
            IAboutSectionReadRepository aboutReadRepository,
            IAboutSectionWriteRepository aboutWriteRepository,
            IAboutSectionDetailReadRepository aboutDetailReadRepository,
            IAboutSectionDetailWriteRepository aboutDetailWriteRepository,
            ILanguageReadRepository languageReadRepository,
            IFileService fileService,
            IHostingEnvironment environment,
            IMapper mapper,
            IValidator<CreateAboutSectionDTO> createAboutSectionValidator)
        {
            _context = context;
            _aboutReadRepository = aboutReadRepository;
            _aboutWriteRepository = aboutWriteRepository;
            _aboutDetailReadRepository = aboutDetailReadRepository;
            _aboutDetailWriteRepository = aboutDetailWriteRepository;
            _languageReadRepository = languageReadRepository;
            _fileService = fileService;
            _environment = environment;
            _mapper = mapper;
            _createAboutSectionValidator = createAboutSectionValidator;
        }
        public async Task AddAboutSection(CreateAboutSectionDTO addAboutSectionDTO)
        {
            //bool isAboutSectionExist = await _context.AboutSectionDetails.AnyAsync(x => x.AboutSectionId == addAboutSectionDTO.Id && x.LanguageId == addAboutSectionDTO.LanguageId);

            //if (isAboutSectionExist)
            //{
            //    throw new Exception("This about section already exists for the selected language.");

            //}

            var validationResult = await _createAboutSectionValidator.ValidateAsync(addAboutSectionDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == addAboutSectionDTO.LanguageId);

            if (language == null)
            {
                throw new Exception("Selected language not found.");
            }

            if (addAboutSectionDTO.ImageFile == null)
            {
                throw new Exception("Image file is required.");
            }

            if (!addAboutSectionDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addAboutSectionDTO.ImageFile.CheckFileSize(200))
            {
                throw new Exception("File size is too large. Maximum allowed size is 200MB.");
            }

            string webRootPath = _environment.WebRootPath;
            string imagePath = await _fileService.SaveFilesAsync(addAboutSectionDTO.ImageFile, webRootPath, "client", "assets", "img", "AboutSectionMedias");


            //var aboutSection = new AboutSection
            //{
            //    Id = Guid.NewGuid(),
            //    ImagePath = imagePath.Trim(),
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow.AddHours(4),
            //    UpdatedDate = DateTime.UtcNow.AddHours(4)
            //};

            var aboutSection = _mapper.Map<AboutSection>(addAboutSectionDTO);
            aboutSection.Id = Guid.NewGuid();
            aboutSection.ImagePath = imagePath;
            aboutSection.IsDeleted = false;
            aboutSection.CreatedDate = DateTime.UtcNow.AddHours(4);
            aboutSection.UpdatedDate = DateTime.UtcNow.AddHours(4);

            _aboutWriteRepository.AddAsync(aboutSection);
            await _aboutWriteRepository.SaveChangeAsync();


            //var aboutSectionDetail = new AboutSectionDetail
            //{
            //    Id = Guid.NewGuid(),
            //    Title = addAboutSectionDTO.Title.Trim(),
            //    Description = addAboutSectionDTO.Description.Trim(),
            //    AboutSectionId = aboutSection.Id,
            //    LanguageId = addAboutSectionDTO.LanguageId,
            //    IsDeleted = false,
            //    CreatedDate = DateTime.UtcNow.AddHours(4),
            //    UpdatedDate = DateTime.UtcNow.AddHours(4)
            //};

            var aboutSectionDetail = _mapper.Map<AboutSectionDetail>(addAboutSectionDTO);
            aboutSectionDetail.Id = Guid.NewGuid();
            aboutSectionDetail.AboutSectionId = aboutSection.Id;
            aboutSectionDetail.CreatedDate = DateTime.UtcNow.AddHours(4);
            aboutSectionDetail.UpdatedDate = DateTime.UtcNow.AddHours(4);

            _aboutDetailWriteRepository.AddAsync(aboutSectionDetail);
            await _aboutDetailWriteRepository.SaveChangeAsync();
        }

        public async Task<List<GetAboutSectionDTO>> GetAllAboutSectionsAsync()
        {
            var about = await _aboutReadRepository.GetAll()
                .Include(s => s.AboutSectionDetails)
                .ThenInclude(sd => sd.Language)
                .Select(s => new GetAboutSectionDTO
                {
                    Id = s.Id,
                    IsoCode = s.AboutSectionDetails.FirstOrDefault().Language.Name,
                    Title = s.AboutSectionDetails.FirstOrDefault().Title,
                    Description = s.AboutSectionDetails.FirstOrDefault().Description,
                    ImagePath = s.ImagePath,
                    IsDeleted = s.IsDeleted
                }).ToListAsync();

            return about;
        }

        public async Task<GetAboutSectionDTO> GetAboutSectionById(Guid id, string IsoCode)
        {
            var aboutSection = await _aboutReadRepository.GetAll()
                 .Include(s => s.AboutSectionDetails)
                 .ThenInclude(sd => sd.Language)
                 .Where(s => s.Id == id)
                 .Select(s => new GetAboutSectionDTO
                 {
                     Id = s.Id,
                     IsoCode = s.AboutSectionDetails.FirstOrDefault().Language.Name,
                     LanguageId = s.AboutSectionDetails.FirstOrDefault().Language.Id,
                     Title = s.AboutSectionDetails.FirstOrDefault().Title,
                     Description = s.AboutSectionDetails.FirstOrDefault().Description,
                     ImagePath = s.ImagePath,
                     IsDeleted = s.IsDeleted
                 })
                 .FirstOrDefaultAsync();

            return aboutSection;
        }

        public async Task DeleteAboutSection(Guid id)
        {
            var aboutSection = await _aboutReadRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (aboutSection == null)
            {
                throw new Exception("About Section not found");
            }
            aboutSection.SoftDelete();
            _aboutWriteRepository.Update(aboutSection);

            var aboutSectionDetails = await _aboutDetailReadRepository.GetAll().Where(x => x.AboutSectionId == id).ToListAsync();
            foreach (var detail in aboutSectionDetails)
            {
                detail.SoftDelete();
                _aboutDetailWriteRepository.Update(detail);
            }
            await _aboutWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreAboutSection(Guid id)
        {
            var aboutSection = await _aboutReadRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (aboutSection == null)
            {
                throw new Exception("About Section not found");
            }
            aboutSection.Restore();
            _aboutWriteRepository.Update(aboutSection);

            var aboutSectionDetails = await _aboutDetailReadRepository.GetAll().Where(x => x.AboutSectionId == id).ToListAsync();
            foreach (var detail in aboutSectionDetails)
            {
                detail.Restore();
                _aboutDetailWriteRepository.Update(detail);
            }
            await _aboutWriteRepository.SaveChangeAsync();
        }
       
        public async Task EditAboutSection(Guid id, CreateAboutSectionDTO updateAboutSectionDTO)
        {

            var AboutSection = _aboutReadRepository.GetAll()
           .Include(s => s.AboutSectionDetails)
           .FirstOrDefault(s => s.Id == id);

            if (AboutSection == null)
            {
                throw new Exception("About Section not found.");
            }
            var AboutSectionDetail = AboutSection.AboutSectionDetails
                .FirstOrDefault(sd => sd.AboutSectionId == id);

            if (AboutSectionDetail == null)
            {
                throw new Exception("About Section detail for the selected language not found.");
            }

            var validationResult = await _createAboutSectionValidator.ValidateAsync(updateAboutSectionDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);

            }

            AboutSectionDetail.Title = updateAboutSectionDTO.Title?.Trim() ?? AboutSectionDetail.Title;
            AboutSectionDetail.Description = updateAboutSectionDTO.Description?.Trim() ?? AboutSectionDetail.Description;
            AboutSectionDetail.UpdatedDate = DateTime.UtcNow.AddHours(4);

            if (updateAboutSectionDTO.ImageFile != null)
            {
                if (!updateAboutSectionDTO.ImageFile.CheckFileType("image"))
                {
                    throw new Exception("Invalid file type. Please upload an image.");
                }

                if (!updateAboutSectionDTO.ImageFile.CheckFileSize(10))
                {
                    throw new Exception("File size is too large. Maximum allowed size is 10MB.");
                }

                string webRootPath = _environment.WebRootPath;
                string newImagePath = await _fileService.SaveFilesAsync(updateAboutSectionDTO.ImageFile, webRootPath, "client", "assets", "img", "AboutSectionMedias");

                AboutSection.ImagePath = newImagePath.Trim();
            }

            _aboutWriteRepository.Update(AboutSection);
            _aboutDetailWriteRepository.Update(AboutSectionDetail);
            await _aboutDetailWriteRepository.SaveChangeAsync();

        }

    }
}
