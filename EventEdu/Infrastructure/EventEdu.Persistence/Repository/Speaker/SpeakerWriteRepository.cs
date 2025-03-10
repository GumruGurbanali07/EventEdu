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
    public class SpeakerWriteRepository : WriteRepository<Speaker>, ISpeakerWriteRepository
    {
        public SpeakerWriteRepository(AppDbContext context) : base(context)
        {
        }

		public async Task SoftDeleteSpeakerAsync(Guid speakerId)
		{
			 await Table.Include(x => x.SpeakerDetails).FirstOrDefaultAsync(x => x.Id == speakerId);
		}
		public Task RestoreSpeakerAsync(Guid speakerId)
		{
			throw new NotImplementedException();
		}

		
	}
}
