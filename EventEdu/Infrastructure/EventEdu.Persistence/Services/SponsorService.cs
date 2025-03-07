using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Repositor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Reflection.Metadata;
using System;
using EventEdu.Persistence.Extensions;
using EventEdu.Persistence.Context;
using EventEdu.Application.DTOs.Sponsor;

namespace EventEdu.Persistence.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorReadRepository _sponsorReadRepository;
        private readonly ISponsorWriteRepository _sponsorWriteRepository;
        private readonly ILanguageReadRepository _languageReadRepository;
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _environment;
        private readonly AppDbContext _context;


        public SponsorService(ISponsorReadRepository sponsorReadRepository,
            ISponsorWriteRepository sponsorWriteRepository,
            ILanguageReadRepository languageReadRepository,
            IFileService fileService,
            IHostingEnvironment environment,
             AppDbContext context)
        {
            _sponsorReadRepository = sponsorReadRepository;
            _sponsorWriteRepository = sponsorWriteRepository;
            _languageReadRepository = languageReadRepository;
            _fileService = fileService;
            _environment = environment;
            _context = context;
        }

        public async Task AddSponsor(CreateSponsorDTO addSponsorDTO)
        {
            bool isSponsorExist = await _context.SponsorDetails.AnyAsync(x => x.SponsorName == addSponsorDTO.SponsorName && x.LanguageId == addSponsorDTO.LanguageId);

            if (addSponsorDTO.ImageFile == null)
            {
                throw new Exception("Image file is required.");
            }

            var language = await _context.Languages.FirstOrDefaultAsync(l => l.Id == addSponsorDTO.LanguageId);

            if (language == null)
            {
                throw new Exception("Selected language not found.");
            }

           

            if (!addSponsorDTO.ImageFile.CheckFileType("image"))
            {
                throw new Exception("Invalid file type. Please upload an image.");
            }

            if (!addSponsorDTO.ImageFile.CheckFileSize(10))
            {
                throw new Exception("File size is too large. Maximum allowed size is 10MB.");
            }

            string webRootPath = _environment.WebRootPath;
            string imagePath = await _fileService.SaveFilesAsync(addSponsorDTO.ImageFile, webRootPath, "client", "assets", "img", "sponsorMedias");


            var sponsor = new Sponsor
            {
                Id = Guid.NewGuid(),
                Email = addSponsorDTO.Email.Trim(),
                PhoneNumber = addSponsorDTO.PhoneNumber.Trim(),
                Website = addSponsorDTO.Website.Trim(),
                ImagePath = imagePath.Trim(),
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4)
            };

            _context.Sponsors.Add(sponsor);
            await _context.SaveChangesAsync();


            var sponsorDetail = new SponsorDetail
                {
                    Id = Guid.NewGuid(),
                    SponsorName = addSponsorDTO.SponsorName.Trim(),
                    SponsorDescription = addSponsorDTO.SponsorDescription.Trim(),
                    SponsorId = sponsor.Id, 
                    LanguageId = addSponsorDTO.LanguageId, 
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                    UpdatedDate = DateTime.UtcNow.AddHours(4)
                };

                _context.SponsorDetails.Add(sponsorDetail);
                await _context.SaveChangesAsync();
            }




        public async Task DeleteSponsor(Guid id)
        {
            //var sponsors = await _context.Sponsors.FirstOrDefaultAsync(x => x.Id == id);
            //if (sponsors == null)
            //{
            //    throw new Exception("sponsor not found");
            //}
            //sponsors.SoftDelete();
            //var sponsorDetails = await _context.SponsorDetails.Where(x => x.SponsorId == id).ToListAsync();
            //foreach (var detail in sponsorDetails)
            //{
            //    detail.SoftDelete();
            //}
            //await _context.SaveChangesAsync();
        }

        public Task<GetSponsorDTO> EditSponsor(Guid id, CreateSponsorDTO updateSponsorDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetSponsorDTO>> GetAllSponsorsByLanguageAsync(string isoCode)
        {
            //var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);

            //if (language == null)
            //{
            //    throw new Exception("Invalid language selection.");
            //}

            var sponsors = await _sponsorReadRepository.GetAll()
                .Include(s => s.SponsorsDetail)
                .ThenInclude(sd => sd.Language)
                //.Where(s => s.SponsorsDetail.Any(sd => sd.SponsorId == s.Id && sd.LanguageId == language.Id))
                .Select(s => new GetSponsorDTO
                {
                    Id = s.Id,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Website = s.Website,
                    ImagePath = s.ImagePath,
                    SponsorName = s.SponsorsDetail.FirstOrDefault().SponsorName,
                    SponsorDescription = s.SponsorsDetail.FirstOrDefault().SponsorDescription
                })
        .ToListAsync();


            return sponsors;
        }

        public async Task<GetSponsorDTO> GetSponsorById(Guid id, string isoCode)
        {
            var language = await _languageReadRepository.GetByIsoCodeAsync(isoCode);

            if (language == null)
            {
                throw new Exception("Invalid language selection.");
            }

            var sponsor = await _sponsorReadRepository.GetAll()
                  .Include(s => s.SponsorsDetail)
                  .ThenInclude(sd => sd.Language)
                  .Where(s => s.Id == id)
                  .Select(s => new GetSponsorDTO
                  {
                      Id = s.Id,
                      Email = s.Email,
                      PhoneNumber = s.PhoneNumber,
                      Website = s.Website,
                      ImagePath = s.ImagePath,
                      SponsorName = s.SponsorsDetail.FirstOrDefault().SponsorName,
                      SponsorDescription = s.SponsorsDetail.FirstOrDefault().SponsorDescription
                  })
                  .FirstOrDefaultAsync();

            return sponsor;
        }
    }
}
