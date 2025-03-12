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
    public class SpeakerReadRepository : ReadRepository<Speaker>, ISpeakerReadRepository
    {
        public SpeakerReadRepository(AppDbContext context) : base(context)
        {
        }

		public async Task<List<Speaker>> GetSpeakersByLanguageAsync(Guid languageId)
		{
			return await Table.Where(x => x.SpeakerDetails.Any(x => x.LanguageId == languageId)).ToListAsync();
		}
	}
}
