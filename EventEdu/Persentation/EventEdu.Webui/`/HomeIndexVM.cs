using EventEdu.Application.DTOs.Category;
using EventEdu.Domain.Entities;
using System.Collections.Generic;

namespace EventEdu.Webui.ViewsModels
{
	public class HomeIndexVM
	{
		public List<HeroSection>? HeroSections { get; set; }
		public HeroSectionDetails? HeroSectionDetails { get; set; }

        public List<Category>? Categories { get; set; }
        public List<CategoryDetail>? CategoryDetails { get; set; }
        public List<Event>? Events { get; set; }
        public List<EventDetail>? EventDetails { get; set; }
    }
}
