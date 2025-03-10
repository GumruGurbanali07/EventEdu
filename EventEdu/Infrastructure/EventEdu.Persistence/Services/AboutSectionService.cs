using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
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
        private readonly IAboutSectionReadRepository _aboutReadRepository;
        private readonly IAboutSectionWriteRepository _aboutWriteRepository;
        private readonly ILanguageReadRepository _languageReadRepository;
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _environment;
        private readonly AppDbContext _context;

        public AboutSectionService(IAboutSectionReadRepository aboutReadRepository,
            IAboutSectionWriteRepository aboutWriteRepository,
            ILanguageReadRepository languageReadRepository,
            IFileService fileService,
            IHostingEnvironment environment,
             AppDbContext context)
        {
            _aboutReadRepository = aboutReadRepository;
            _aboutWriteRepository = aboutWriteRepository;
            _languageReadRepository = languageReadRepository;
            _fileService = fileService;
            _environment = environment;
            _context = context;
        }
        public async Task AddAboutSection(CreateAboutSectionDTO addAboutSectionDTO)
        {
            bool isAboutSectionExist = await _context.AboutSectionDetails.AnyAsync(x => x.AboutSectionId == addAboutSectionDTO.Id && x.LanguageId == addAboutSectionDTO.LanguageId);

            if (isAboutSectionExist)
            {
                throw new Exception("This about section already exists for the selected language.");

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

            if (!addAboutSectionDTO.ImageFile.CheckFileSize(10))
            {
                throw new Exception("File size is too large. Maximum allowed size is 10MB.");
            }

            string webRootPath = _environment.WebRootPath;
            string imagePath = await _fileService.SaveFilesAsync(addAboutSectionDTO.ImageFile, webRootPath, "client", "assets", "img", "AboutSectionMedias");


            var aboutSection = new AboutSection
            {
                Id = Guid.NewGuid(),
                ImagePath = imagePath.Trim(),
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4)
            };

            _context.AboutSections.Add(aboutSection);
            await _context.SaveChangesAsync();


            var aboutSectionDetail = new AboutSectionDetail
            {
                Id = Guid.NewGuid(),
                Title = addAboutSectionDTO.Title.Trim(),
                Description = addAboutSectionDTO.Description.Trim(),
                AboutSectionId = aboutSection.Id,
                LanguageId = addAboutSectionDTO.LanguageId,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4)
            };

            _context.AboutSectionDetails.Add(aboutSectionDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetAboutSectionDTO>> GetAllAboutSectionsAsync()
        {
            var about = await _aboutReadRepository.GetAll()
                .Include(s => s.AboutSectionDetails)
                .ThenInclude(sd => sd.Language)
                .Select(s => new GetAboutSectionDTO
                {
                    Id = s.Id,
                    Title = s.AboutSectionDetails.FirstOrDefault().Title,
                    Description = s.AboutSectionDetails.FirstOrDefault().Description,
                    ImagePath = s.ImagePath,
                    IsDeleted = s.IsDeleted
                }).ToListAsync();

            return about;
        }

        public async Task<GetAboutSectionDTO> GetAboutSectionById(Guid id)
        {
            var aboutSection = await _aboutReadRepository.GetAll()
                 .Include(s => s.AboutSectionDetails)
                 .ThenInclude(sd => sd.Language)
                 .Where(s => s.Id == id)
                 .Select(s => new GetAboutSectionDTO
                 {
                     Id = s.Id,
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
            var aboutSection = await _context.AboutSections.FirstOrDefaultAsync(x => x.Id == id);
            if (aboutSection == null)
            {
                throw new Exception("About Section not found");
            }
            aboutSection.SoftDelete();
            var aboutSectionDetails = await _context.AboutSectionDetails.Where(x => x.AboutSectionId == id).ToListAsync();
            foreach (var detail in aboutSectionDetails)
            {
                detail.SoftDelete();
            }
            await _aboutWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreAboutSection(Guid id)
        {
            var aboutSection = await _context.AboutSections.FirstOrDefaultAsync(x => x.Id == id);
            if (aboutSection == null)
            {
                throw new Exception("About Section not found");
            }
            aboutSection.Restore();
            var aboutSectionDetails = await _context.AboutSectionDetails.Where(x => x.AboutSectionId == id).ToListAsync();
            foreach (var detail in aboutSectionDetails)
            {
                detail.Restore();
            }
            await _aboutWriteRepository.SaveChangeAsync();
        }
       
        public async Task<CreateAboutSectionDTO> EditAboutSection(Guid id, CreateAboutSectionDTO updateAboutSectionDTO)
        {
            var AboutSection = _context.AboutSections
           .Include(s => s.AboutSectionDetails)
           .FirstOrDefault(s => s.Id == updateAboutSectionDTO.Id);

            if (AboutSection == null)
            {
                throw new Exception("About Section not found.");
            }
            var AboutSectionDetail = AboutSection.AboutSectionDetails
                .FirstOrDefault(sd => sd.AboutSectionId == updateAboutSectionDTO.Id);

            if (AboutSectionDetail == null)
            {
                throw new Exception("About Section detail for the selected language not found.");
            }

            AboutSection.UpdatedDate = DateTime.UtcNow.AddHours(4);

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

            _context.AboutSections.Update(AboutSection);
            _context.AboutSectionDetails.Update(AboutSectionDetail);
            await _context.SaveChangesAsync();

            var getAboutSectionDTO = new GetAboutSectionDTO
            {
                Id = AboutSection.Id,
                Title = AboutSectionDetail.Title,
                Description = AboutSectionDetail.Description,
                ImagePath = AboutSection.ImagePath
            };

            return updateAboutSectionDTO;
        }


    }
}
