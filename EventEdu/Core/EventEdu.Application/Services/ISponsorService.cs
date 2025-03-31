using EventEdu.Application.DTOs.Sponsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface ISponsorService
    {
        Task AddSponsor(CreateSponsorDTO addSponsorDTO);
        Task<List<GetSponsorDTO>> GetAllSponsorsByLanguageAsync(string isoCode);

        Task<List<SponsorDetail>> GetSponsorsById(string eventId);
        Task<(List<Sponsor>, List<SponsorDetail>)> GetSponsorAll();

		Task<GetSponsorDTO> GetSponsorById(Guid id, string isoCode);
        Task EditSponsor(Guid id, CreateSponsorDTO updateSponsorDTO);
        Task DeleteSponsor(Guid id);
        Task RestoreSponsor(Guid id);
        Task<List<GetSponsorDTO>> SearchSponsors(string search);
    }
}
