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
    public class FeedBackDetailReadRepository : ReadRepository<FeedBackDetail>, IFeedBackDetailReadRepository
    {
		public FeedBackDetailReadRepository(AppDbContext context) : base(context)
		{
		}
		public async Task<List<FeedBackDetail>> GetFeedbacksByEventAndLanguageAsync(Guid eventId, string isoCode)
		{
			return await Table
				.Include(fd => fd.FeedBack) 
				.Include(fd => fd.Language) 
				.Where(fd => fd.FeedBack.EventId == eventId && fd.Language.IsoCode == isoCode)
				.ToListAsync();
		}
	}
    
}
