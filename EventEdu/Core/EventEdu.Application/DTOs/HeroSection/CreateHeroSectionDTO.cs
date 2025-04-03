using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.HeroSection
{
    public class CreateHeroSectionDTO
    {
        public Guid Id { get; set; }
        public Guid LanguageId { get; set; }

        public List<SelectListItem>? Language { get; set; }  
        public string? ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
