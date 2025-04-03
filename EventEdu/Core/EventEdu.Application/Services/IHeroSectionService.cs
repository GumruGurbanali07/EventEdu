using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IHeroSectionService
    {
        Task<(List<HeroSection>, HeroSectionDetails)> GetHeroSectionAll();
        Task AddSlider(CreateHeroSectionDTO addSliderDTO);
        Task<List<GetHeroSectionDTO>> GetAllSlidersAsync();
        Task<GetHeroSectionDTO> GetSLiderById(Guid id, string isoCode);
        Task EditSlider(Guid id, CreateHeroSectionDTO updateSliderDTO);
        Task DeleteSlider(Guid id);
        Task RestoreSlider(Guid id);
    }
}
