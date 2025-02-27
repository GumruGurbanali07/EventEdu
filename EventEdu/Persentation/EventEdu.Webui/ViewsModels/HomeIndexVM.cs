using EventEdu.Domain.Entities;
using System.Collections.Generic;

namespace EventEdu.Webui.ViewsModels
{
	public class HomeIndexVM
	{
		public List<HeroSection> HeroSections { get; set; }
		public List<Category> Categories { get; set; }
	}
}
