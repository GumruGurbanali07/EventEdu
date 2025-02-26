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
	public class LanguageReadRepository : ReadRepository<Language>, ILanguageReadRepository
	{
		public LanguageReadRepository(AppDbContext context) : base(context)
		{
		}
		public async Task<Language> GetByIsoCodeAsync(string isoCode)
		{
			return await _context.Languages.FirstOrDefaultAsync(x => x.IsoCode == isoCode);
		}

	}
}
