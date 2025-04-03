using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

		public async Task<SpeakerDetail> GetBySpeakerIdAndLanguageIdAsync(Guid speakerId, Guid languageId,  bool traking=true)
		{
			var query = traking ? Table.AsQueryable() : Table.AsNoTracking();


			return await query.FirstOrDefaultAsync(x => x.Id == speakerId && x.LanguageId == languageId);
		}
		public async Task<SpeakerDetail> GetBySpeakerIdAsync(Guid speakerId, bool traking = true)
		{
			var query = traking ? Table.AsQueryable() : Table.AsNoTracking();

			return await query.FirstOrDefaultAsync(x => x.Id == speakerId);
		}
		public async Task<List<SpeakerDetail>> GetByLanguageIdAsync(Guid languageId , bool traking = true)
		{
			var query = traking ? Table.AsQueryable() : Table.AsNoTracking();

			return await query.Where(x => x.LanguageId == languageId).ToListAsync();
		}
	}
}
