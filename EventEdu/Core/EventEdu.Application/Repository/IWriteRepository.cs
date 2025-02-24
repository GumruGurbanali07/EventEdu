using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Repository
{
	public interface IWriteRepository<T>:IRepository<T> where T:BaseEntity
	{
		Task<bool> AddAsync(T model);
		Task<bool> Remove(string id);
		bool Update(T model);
		bool Remove(T model);
		Task<int> SaveChangeAsync();
	}
}
