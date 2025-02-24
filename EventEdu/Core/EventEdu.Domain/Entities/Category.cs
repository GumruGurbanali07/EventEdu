using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class Category:BaseEntity
	{
		public ICollection<CategoryDetail> CategoryDetail { get; set; } 

	}
}
