using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.Sponsor
{
    public class CreateSponsorDTO
    {
        public string IsoCode { get; set; }
        public Guid LanguageId { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Website { get; set; } 
        public string ImagePath { get; set; }
        public ICollection<SponsorDetail> SponsorDetail { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
