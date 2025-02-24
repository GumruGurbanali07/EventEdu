using EventEdu.Application.Repository;
using EventEdu.Domain.Entities.Common;
using EventEdu.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Repository
{
	public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
	{
		private readonly AppDbContext _context;

		public WriteRepository(AppDbContext context)
		{
			_context = context;
		}

		public DbSet<T> Table => _context.Set<T>();
		public async Task<bool> AddAsync(T model)
		{
			EntityEntry<T> entry = await Table.AddAsync(model);
			return entry.State == EntityState.Added;
		}

		public async Task<bool> Remove(string id)
		{
			T values = await Table.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
			return Remove(values);
		}

		public bool Remove(T model)
		{
			EntityEntry<T> entry = Table.Remove(model);
			return entry.State == EntityState.Deleted;
		}

		public async Task<int> SaveChangeAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public bool Update(T model)
		{
			EntityEntry entry = Table.Update(model);
			return entry.State == EntityState.Modified;
		}
	}
}
