
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
	public interface ILanguageService
	{
		Task<Language> CreateAsync(CreateLanguageDTO languageDTO);
		Task AddLanguageAsync(LanguageViewModel languageViewModel);
		Task<List<LanguageViewModel>> GetLanguagesAsync();
		Task<LanguageViewModel> GetLanguageAsync(string isoCode);
	}
}
