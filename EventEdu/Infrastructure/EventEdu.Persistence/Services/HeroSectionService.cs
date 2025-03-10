using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.Repositor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
using EventEdu.Persistence.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
    public class HeroSectionService : IHeroSectionService
    {
        private readonly IHeroSectionReadRepository _sliderReadRepository;
        private readonly IHeroSectionWriteRepository _sliderWriteRepository;
        private readonly ILanguageReadRepository _languageReadRepository;
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _environment;
        private readonly AppDbContext _context;


        public HeroSectionService(IHeroSectionReadRepository sliderReadRepository,
            IHeroSectionWriteRepository sliderWriteRepository,
            ILanguageReadRepository languageReadRepository,
            IFileService fileService,
            IHostingEnvironment environment,
             AppDbContext context)
        {
            _sliderReadRepository = sliderReadRepository;
            _sliderWriteRepository = sliderWriteRepository;
            _languageReadRepository = languageReadRepository;
            _fileService = fileService;
            _environment = environment;
            _context = context;
        }
        public async Task AddSlider(CreateHeroSectionDTO addSliderDTO)
        {
            bool isSliderExist = await _context.HeroSectionDetails.AnyAsync(x => x.HeroSectionId == addSliderDTO.Id && x.LanguageId == addSliderDTO.LanguageId);

            if (isSliderExist)
            {
                throw new Exception("This slider already exists for the selected language.");

            }

            var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == addSliderDTO.LanguageId);

            if (language == null)
            {
                throw new Exception("Selected language not found.");
            }

            if (addSliderDTO.ImageFile == null)
            {
                throw new Exception("Image file is required.");
            }

            if (!addSliderDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addSliderDTO.ImageFile.CheckFileSize(10))
            {
                throw new Exception("File size is too large. Maximum allowed size is 10MB.");
            }

            string webRootPath = _environment.WebRootPath;
            string imagePath = await _fileService.SaveFilesAsync(addSliderDTO.ImageFile, webRootPath, "client", "assets", "img", "SliderMedias");


            var slider = new HeroSection
            {
                Id = Guid.NewGuid(),
                ImagePath = imagePath.Trim(),
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4)
            };

            _context.HeroSections.Add(slider);
            await _context.SaveChangesAsync();


            var sliderDetail = new HeroSectionDetails
            {
                Id = Guid.NewGuid(),
                Title = addSliderDTO.Title.Trim(),
                Description = addSliderDTO.Description.Trim(),
                HeroSectionId = slider.Id,
                LanguageId = addSliderDTO.LanguageId,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4)
            };

            _context.HeroSectionDetails.Add(sliderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetHeroSectionDTO>> GetAllSlidersAsync()
        {
            var sliders = await _sliderReadRepository.GetAll()
               .Include(s => s.HeroSectionDetails)
               .ThenInclude(sd => sd.Language)
               .Select(s => new GetHeroSectionDTO
               {
                   Id = s.Id,
                   Title = s.HeroSectionDetails.FirstOrDefault().Title,
                   Description = s.HeroSectionDetails.FirstOrDefault().Description,
                   ImagePath = s.ImagePath,
                   IsDeleted = s.IsDeleted
               }).ToListAsync();

            return sliders;
        }

        public async Task<GetHeroSectionDTO> GetSLiderById(Guid id)
        {

            var slider = await _sliderReadRepository.GetAll()
                  .Include(s => s.HeroSectionDetails)
                  .ThenInclude(sd => sd.Language)
                  .Where(s => s.Id == id)
                  .Select(s => new GetHeroSectionDTO
                  {
                      Id = s.Id,
                      Title = s.HeroSectionDetails.FirstOrDefault().Title,
                      Description = s.HeroSectionDetails.FirstOrDefault().Description,
                      ImagePath = s.ImagePath,
                      IsDeleted = s.IsDeleted
                  })
                  .FirstOrDefaultAsync();

            return slider;
        }

        public async Task DeleteSlider(Guid id)
        {
            var slider = await _context.HeroSections.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                throw new Exception("Slider not found");
            }
            slider.SoftDelete();
            var sliderDetails = await _context.HeroSectionDetails.Where(x => x.HeroSectionId == id).ToListAsync();
            foreach (var detail in sliderDetails)
            {
                detail.SoftDelete();
            }
            await _sliderWriteRepository.SaveChangeAsync();
        }

        public async Task RestoreSlider(Guid id)
        {
            var slider = await _context.HeroSections.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                throw new Exception("Slider not found");
            }
            slider.Restore();
            var sliderDetails = await _context.HeroSectionDetails.Where(x => x.HeroSectionId == id).ToListAsync();
            foreach (var detail in sliderDetails)
            {
                detail.Restore();
            }
            await _sliderWriteRepository.SaveChangeAsync();
        }

        public async Task<CreateHeroSectionDTO> EditSlider(Guid id, CreateHeroSectionDTO updateSliderDTO)
        {
            var Slider = _context.HeroSections
           .Include(s => s.HeroSectionDetails)
           .FirstOrDefault(s => s.Id == updateSliderDTO.Id);

            if (Slider == null)
            {
                throw new Exception("Slider not found.");
            }
            var SliderDetail = Slider.HeroSectionDetails
                .FirstOrDefault(sd => sd.HeroSectionId == updateSliderDTO.Id);

            if (SliderDetail == null)
            {
                throw new Exception("Slider detail for the selected language not found.");
            }

            Slider.UpdatedDate = DateTime.UtcNow.AddHours(4);

            SliderDetail.Title = updateSliderDTO.Title?.Trim() ?? SliderDetail.Title;
            SliderDetail.Description = updateSliderDTO.Description?.Trim() ?? SliderDetail.Description;
            SliderDetail.UpdatedDate = DateTime.UtcNow.AddHours(4);

            if (updateSliderDTO.ImageFile != null)
            {
                if (!updateSliderDTO.ImageFile.CheckFileType("image"))
                {
                    throw new Exception("Invalid file type. Please upload an image.");
                }

                if (!updateSliderDTO.ImageFile.CheckFileSize(10))
                {
                    throw new Exception("File size is too large. Maximum allowed size is 10MB.");
                }

                string webRootPath = _environment.WebRootPath;
                string newImagePath = await _fileService.SaveFilesAsync(updateSliderDTO.ImageFile, webRootPath, "client", "assets", "img", "sliderMedias");

                Slider.ImagePath = newImagePath.Trim();
            }

            _context.HeroSections.Update(Slider);
            _context.HeroSectionDetails.Update(SliderDetail);
            await _context.SaveChangesAsync();

            var getSliderDTO = new GetHeroSectionDTO
            {
                Id = Slider.Id,
                Title = SliderDetail.Title,
                Description = SliderDetail.Description,
                ImagePath = Slider.ImagePath
            };

            return updateSliderDTO;
        }
    }

}
