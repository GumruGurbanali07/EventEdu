using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Repository
{
    public interface ISpeakerDetailReadRepository : IReadRepository<SpeakerDetail>
    {
        Task<SpeakerDetail> GetBySpeakerIdAndLanguageIdAsync(Guid speakerId, Guid languageId, bool tracking=true);
        Task<List<SpeakerDetail>> GetByLanguageIdAsync(Guid languageId, bool tracking = true);
        Task<SpeakerDetail> GetBySpeakerIdAsync(Guid speakerId, bool tracking = true);
	}
}
