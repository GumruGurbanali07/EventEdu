using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Social
{
	public class CreateSocialDto
	{
		public string IconUrl { get; set; }
		public string Title { get; set; }
		public string WebUrl { get; set; }
		[NotMapped]
		public IFormFile FormFile { get; set; }

	}
}
