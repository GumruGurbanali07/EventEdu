using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
	public class Language:BaseEntity
	{
		public string Name { get; set; } = null!;
		public string IsoCode { get; set; } = null!;
		public string ImagePath { get; set; } = null!;
	}
}
