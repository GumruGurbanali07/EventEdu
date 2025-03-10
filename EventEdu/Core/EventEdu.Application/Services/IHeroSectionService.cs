using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.DTOs.Sponsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IHeroSectionService
    {
        Task AddSlider(CreateHeroSectionDTO addSliderDTO);
        Task<List<GetHeroSectionDTO>> GetAllSlidersAsync();
        Task<GetHeroSectionDTO> GetSLiderById(Guid id);
        Task<CreateHeroSectionDTO> EditSlider(Guid id, CreateHeroSectionDTO updateSliderDTO);
        Task DeleteSlider(Guid id);
        Task RestoreSlider(Guid id);
    }
}
