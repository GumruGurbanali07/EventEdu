using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.HeroSection
{
    public class CreateHeroSectionDTO
    { //Mediani include edersen
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid LanguageId { get; set; }
    }
}
