using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities.Common
{  
	public class BaseEntity : ISoftDeletable
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public bool IsDeleted { get; set; }

        public virtual void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.UtcNow.AddHours(4);
        }

        public virtual void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.UtcNow.AddHours(4);
        }
    }
}