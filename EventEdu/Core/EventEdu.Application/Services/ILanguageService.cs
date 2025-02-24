using EventEdu.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
	public interface ILanguageService
	{
		Task<List<LanguageViewModel>> GetLanguagesAsync();
		Task<LanguageViewModel> GetLanguageAsync(string isoCode);
	}
}
