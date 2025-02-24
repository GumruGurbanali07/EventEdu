using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities
{
    public class NotificationDetail:BaseEntity
    {
		public string Message { get; set; }
		public Guid NotificationId { get; set; }
		public Notification Notification { get; set; }

		public Guid LanguageId { get; set; }
		public Language Language { get; set; }

	}
}
