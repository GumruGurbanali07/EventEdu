using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Category
{
    public class GetCategoryDTO
    {
		public Guid Id { get; set; }
		public string CategoryName { get; set; }
		public string IsoCode { get; set; }  // ISO code (az-AZ, en-US, ru-RU)

		public bool IsDeleted { get; set; }	
	}
}
