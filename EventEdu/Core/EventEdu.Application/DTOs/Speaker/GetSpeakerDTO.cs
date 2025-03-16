using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Speaker
{
	public class GetSpeakerDTO
	{
		public Guid Id { get; set; }
		public string FullName { get; set; }
		public string Bio { get; set; }
		public string IsoCode { get; set; }  // ISO code (az-AZ, en-US, ru-RU)
		public string ImagePath { get; set; }
		public string Email { get; set; }
		public string FacebookLink { get; set; }
		public string TwitterLink { get; set; }
		public string InstagramLink { get; set; }
        public bool IsDeleted { get; set; }
    }
}
