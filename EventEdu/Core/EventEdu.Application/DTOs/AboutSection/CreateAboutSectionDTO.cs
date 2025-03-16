using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.DTOs.AboutSection
{
    public class CreateAboutSectionDTO
    {
        public Guid Id { get; set; }
        public Guid LanguageId { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public List<string> Features { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
