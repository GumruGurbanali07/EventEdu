using EventEdu.Application.DTOs.Category;
using EventEdu.Domain.Entities;
using System.Collections.Generic;

namespace EventEdu.Webui.ViewsModels
{
	public class HomeIndexVM
	{
		public List<HeroSection>? HeroSections { get; set; }
		public List<CategoryDetail>? Categories { get; set; }

	}
}
