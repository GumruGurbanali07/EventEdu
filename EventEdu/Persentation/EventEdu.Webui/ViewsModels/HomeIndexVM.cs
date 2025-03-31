using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.DTOs.Category;
using EventEdu.Domain.Entities;
using System.Collections.Generic;

namespace EventEdu.Webui.ViewsModels
{
	public class HomeIndexVM
	{
		public List<HeroSection> HeroSections { get; set; }
		public List<Category> Categories { get; set; }
        public List<GetSponsorDTO> Sponsors { get; set; }
        public List<GetAboutSectionDTO> AboutSection { get; set; }
		public List<CategoryDetail>? Categories { get; set; }

	}
}
