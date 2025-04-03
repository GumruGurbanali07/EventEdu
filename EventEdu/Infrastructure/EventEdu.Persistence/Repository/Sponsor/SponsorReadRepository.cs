using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Repositor;
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
    public class SponsorReadRepository : ReadRepository<Sponsor>, ISponsorReadRepository
    {
        public SponsorReadRepository(AppDbContext context) : base(context)
        {
        }

        //public Task<List<GetSponsorDTO>> Search(string search)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<List<GetSponsorDTO>> Search(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query))
        //        return new List<GetSponsorDTO>();

        //    query = query.ToLower();

        //    return await _context.Sponsors
        //        .Where(s => !s.IsDeleted &&
        //                    s.SponsorsDetail.Any(sd =>
        //                        sd.SponsorName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
        //                        sd.SponsorDescription.Contains(query, StringComparison.OrdinalIgnoreCase)))
        //        .Select(s => new GetSponsorDTO
        //        {
        //            SponsorName = s.SponsorsDetail.FirstOrDefault().SponsorName, // Assuming you want the first sponsor detail here
        //            ImagePath = s.ImagePath
        //        })
        //        .ToListAsync();
        //}
    }
}
