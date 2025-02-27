using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventEdu.Persistence.Repository
{
    public class HeroSectionDetailWriteRepository : WriteRepository<HeroSectionDetail>, IHeroSectionDetailWriteRepository
    {
        public HeroSectionDetailWriteRepository(AppDbContext context) : base(context)
        {
        }
    }
}
