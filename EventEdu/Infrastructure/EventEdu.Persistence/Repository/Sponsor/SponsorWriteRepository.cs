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
    public class SponsorWriteRepository : WriteRepository<Sponsor>, ISponsorWriteRepository
    {
        public SponsorWriteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task RestoreSponsor(Guid sponsorId)
        {
            await Table.Include(x => x.SponsorsDetail).FirstOrDefaultAsync(x => x.Id == sponsorId);
        }

        public Task SoftDeleteSponsor(Guid sponsorId)
        {
            throw new NotImplementedException();
        }
    }
}
