using EventEdu.Domain.Entities;

namespace EventEdu.Webui.ViewsModels
{
	public class CategoryLanguageVM
	{
		public List<CategoryDetail> CategoryDetails { get; set; }
		public List<Language> Languages { get; set; }
	}
}
