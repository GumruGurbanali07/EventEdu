using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Repositor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

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
                var language = await _languageReadRepository.GetByIsoCodeAsync(addSponsorDTO.IsoCode);

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
                SponsorDetail = sponsor.SponsorsDetail.Select(sd => new SponsorDetail
                {
                    SponsorName = sd.SponsorName,
                    SponsorDescription = sd.SponsorDescription,
                    LanguageId = sd.LanguageId
                }).ToList()

            };
        }

        public Task<GetSponsorDTO> EditSponsor(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetSponsorDTO>> GetAllSponsorsByLanguageAsync(string isoCode)
        {
            var sponsors = await _sponsorReadRepository.GetAll()
       .Include(s => s.SponsorsDetail)
       .ThenInclude(sd => sd.Language)
       .Where(s => s.SponsorsDetail.Any(sd => sd.Language.IsoCode == isoCode))
       .Select(s => new GetSponsorDTO
       {
           Id = s.Id,
           Email = s.Email,
           PhoneNumber = s.PhoneNumber,
           Website = s.Website,
           ImagePath = s.ImagePath
       }).ToListAsync();

            return sponsors;
        }

        public async Task<GetSponsorDTO> GetSponsorById(Guid id, string isoCode)
        {
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
                     ImagePath = s.ImagePath
                 }).FirstOrDefaultAsync();

            return sponsor;
        }
    }
}
