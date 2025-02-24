using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.ViewModel
{
	public class CurrenLanguageViewModel
	{
		public List<LanguageViewModel>? Languages { get; set; }
		public LanguageViewModel? SelectedLanguage { get; set; }
	}
}
