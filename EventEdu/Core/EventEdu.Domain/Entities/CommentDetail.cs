using EventEdu.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventEdu.Domain.Entities
{
	public class CommentDetail : BaseEntity
	{
		public string Text { get; set; }
		public Guid CommentId { get; set; }  
		public  Comment Comment { get; set; }

		public Guid LanguageId { get; set; } 
		public  Language Language { get; set; }  
	}
}
