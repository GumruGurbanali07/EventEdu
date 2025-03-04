using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
    public class SponsorService : ISponsorService
    {
        public Task AddSponsorAsync(CreateSponsorDTO addSponsorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<GetSponsorDTO> DeleteSponsor(Guid id, string webRootPath)
        {
            throw new NotImplementedException();
        }

        public Task<List<GetSponsorDTO>> GetAllSponsorsByLanguageAsync(string isoCode)
        {
            throw new NotImplementedException();
        }

        public Task<GetSponsorDTO> GetSponsorById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
