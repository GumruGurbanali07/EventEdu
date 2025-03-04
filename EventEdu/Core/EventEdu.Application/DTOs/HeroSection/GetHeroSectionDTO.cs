using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.HeroSection
{
    public class GetHeroSectionDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IsoCode { get; set; }  // ISO code (az-AZ, en-US, ru-RU)
        public string ImagePath { get; set; }
    }
}
