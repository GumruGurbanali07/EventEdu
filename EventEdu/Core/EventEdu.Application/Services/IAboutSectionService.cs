using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IAboutSectionService
    {
        Task AddAboutSection(CreateAboutSectionDTO addAboutSectionDTO);
        Task<List<GetAboutSectionDTO>> GetAllAboutSectionsAsync();

        Task<(AboutSection, AboutSectionDetail)> GetAboutSectionAll();
        Task<(AboutSection, AboutSectionDetail)> GetAboutSectionById(Guid id);
        Task EditAboutSection(Guid id, CreateAboutSectionDTO updateAboutSectionDTO);
        Task DeleteAboutSection(Guid id);
        Task RestoreAboutSection(Guid id);
    }
}
