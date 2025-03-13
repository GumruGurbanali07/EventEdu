using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Repository
{
    public class SpeakerDetailReadRepository : ReadRepository<SpeakerDetail>, ISpeakerDetailReadRepository
    {
        public SpeakerDetailReadRepository(AppDbContext context) : base(context)
        {
        }

		public async Task<SpeakerDetail> GetBySpeakerIdAndLanguageIdAsync(Guid speakerId, Guid languageId)
		{
			return await Table.FirstOrDefaultAsync(x => x.SpeakerId == speakerId && x.LanguageId == languageId);
		}
		public async Task<SpeakerDetail> GetBySpeakerIdAsync(Guid speakerId)
		{
			return await Table.FirstOrDefaultAsync(x => x.SpeakerId == speakerId);
		}
		public async Task<List<SpeakerDetail>> GetByLanguageIdAsync(Guid languageId)
		{
			return await Table.Where(x => x.LanguageId == languageId).ToListAsync();
		}
	}
}
