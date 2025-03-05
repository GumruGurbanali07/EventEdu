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

namespace EventEdu.Persistence.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorReadRepository _sponsorReadRepository;
        private readonly ISponsorWriteRepository _sponsorWriteRepository;
        private readonly ILanguageReadRepository _languageReadRepository;


        public SponsorService(ISponsorReadRepository sponsorReadRepository,
            ISponsorWriteRepository sponsorWriteRepository,
            ILanguageReadRepository languageReadRepository)
        {
            _sponsorReadRepository = sponsorReadRepository;
            _sponsorWriteRepository = sponsorWriteRepository;
            _languageReadRepository = languageReadRepository;
        }

        public async Task AddSponsorAsync(CreateSponsorDTO addSponsorDTO)
        {
            bool isSponsorExist = await _sponsorReadRepository.GetAll()
             .Include(s => s.SponsorsDetail)
             .ThenInclude(sd => sd.Language)
             .AnyAsync(s => s.SponsorsDetail.Any(sd => sd.SponsorName == addSponsorDTO.SponsorDetail.FirstOrDefault().SponsorName
                                                                      && sd.LanguageId == addSponsorDTO.LanguageId));

            if (isSponsorExist)
            {
                throw new Exception("This sponsor already exists for the selected language.");
            }

            var sponsor = new Sponsor
            {
                Id = Guid.NewGuid(),
                Email = addSponsorDTO.Email,
                PhoneNumber = addSponsorDTO.PhoneNumber,
                Website = addSponsorDTO.Website,
                ImagePath = addSponsorDTO.ImagePath,
                SponsorsDetail = new List<SponsorDetail>()
            };

            foreach (var detail in addSponsorDTO.SponsorDetail)
            {
                var language = await _languageReadRepository.GetByIdAsync(addSponsorDTO.LanguageId);

                if (language == null)
                {
                    throw new Exception("Invalid language selection.");
                }

                sponsor.SponsorsDetail.Add(new SponsorDetail
                {
                    SponsorId = sponsor.Id,
                    LanguageId = language.Id,
                    SponsorName = detail.SponsorName,
                    SponsorDescription = detail.SponsorDescription
                });
            }

            await _sponsorWriteRepository.AddAsync(sponsor);
            await _sponsorWriteRepository.SaveChangeAsync();
        }

        public async Task<GetSponsorDTO> DeleteSponsor(Guid id)
        {
            var sponsor = await _sponsorReadRepository.GetAll()
        .Include(s => s.SponsorsDetail)
        .FirstOrDefaultAsync(s => s.Id == id);

            if (sponsor == null)
                throw new Exception("Sponsor not found.");

            _sponsorWriteRepository.Remove(sponsor);

            return new GetSponsorDTO
            {
                Id = sponsor.Id,
                Email = sponsor.Email,
                PhoneNumber = sponsor.PhoneNumber,
                Website = sponsor.Website,
                ImagePath = sponsor.ImagePath,
                SponsorName = sponsor.SponsorsDetail.FirstOrDefault().SponsorName,
                SponsorDescription = sponsor.SponsorsDetail.FirstOrDefault().SponsorDescription
            };
        }

        public async Task<GetSponsorDTO> EditSponsor(Guid id, CreateSponsorDTO updateSponsorDTO)
        {
            var sponsor = await _sponsorReadRepository.GetAll()
                .Include(s => s.SponsorsDetail)
                .ThenInclude(sd => sd.Language)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sponsor == null)
            {
                throw new Exception("Sponsor not found.");
            }

            bool isSponsorExist = await _sponsorReadRepository.GetAll()
            .Include(s => s.SponsorsDetail)
            .ThenInclude(sd => sd.Language)
            .AnyAsync(s => s.SponsorsDetail.Any(sd => sd.SponsorName == updateSponsorDTO.SponsorDetail.FirstOrDefault().SponsorName
                                                                     && sd.LanguageId == updateSponsorDTO.LanguageId));

            if (isSponsorExist)
            {
                throw new Exception("This sponsor already exists for the selected language.");
            }

            sponsor.Email = updateSponsorDTO.Email;
            sponsor.PhoneNumber = updateSponsorDTO.PhoneNumber;
            sponsor.Website = updateSponsorDTO.Website;
            sponsor.ImagePath = updateSponsorDTO.ImagePath;


            sponsor.SponsorsDetail.Clear();

            foreach (var detail in updateSponsorDTO.SponsorDetail)
            {

                if (detail.LanguageId == Guid.Empty)
                {
                    throw new Exception("Language ID is required.");
                }

                var language = await _languageReadRepository.GetByIdAsync(detail.LanguageId);
                if (language == null)
                {
                    throw new Exception($"Invalid language selection: {detail.LanguageId}");
                }

                sponsor.SponsorsDetail.Add(new SponsorDetail
                {
                    SponsorId = sponsor.Id,
                    LanguageId = language.Id,
                    SponsorName = detail.SponsorName,
                    SponsorDescription = detail.SponsorDescription
                });
            }

            _sponsorWriteRepository.Update(sponsor);
            await _sponsorWriteRepository.SaveChangeAsync();

            return new GetSponsorDTO
            {
                Id = sponsor.Id,
                Email = sponsor.Email,
                PhoneNumber = sponsor.PhoneNumber,
                Website = sponsor.Website,
                ImagePath = sponsor.ImagePath,
                SponsorName = sponsor.SponsorsDetail.FirstOrDefault().SponsorName,
                SponsorDescription = sponsor.SponsorsDetail.FirstOrDefault().SponsorDescription
            };
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
