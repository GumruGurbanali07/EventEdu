using EventEdu.Application.Repository;
using EventEdu.Domain.Entities.Common;
using EventEdu.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Repository
{
	public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
	{
		public readonly AppDbContext _context;

		public ReadRepository(AppDbContext context)
		{
			_context = context;
		}
		public DbSet<T> Table => _context.Set<T>();
		public IQueryable<T> GetAll(bool tracking = true)
		{
			var query=Table.AsQueryable();
			if (!tracking) 
				query=Table.AsNoTracking();
			return query;
		}

		public async Task<T> GetByIdAsync(string id,bool tracking=true)
		{
			var query = Table.AsQueryable();
			if(!tracking)
				query=Table.AsNoTracking();
			return await query.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
		}
	}
}
