using EventEdu.Application.DTOs.Category;
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
        Task<GetSponsorDTO> GetSponsorById(Guid id, string isoCode);
        Task<GetSponsorDTO> EditSponsor(Guid id, CreateSponsorDTO updateSponsorDTO);
        Task DeleteSponsor(Guid id);
    }
}
