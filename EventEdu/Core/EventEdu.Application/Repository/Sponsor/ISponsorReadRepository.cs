using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Repositor
{
    public interface ISponsorReadRepository : IReadRepository<Sponsor>
    {
        //Task<List<Sponsor>> GetSponsorsByLanguageIdAsync(Guid languageId);

        //Task<List<GetSponsorDTO>> Search(string search);

    }
}
