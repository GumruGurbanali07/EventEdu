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
		Task<List<Language>> GetAllLanguagesAsync();
		Task<Language?> GetLanguageByIdAsync(string id);
		Task<bool> AddLanguageAsync(LanguageAddDTO languageAddDTO);
		Task<bool> UpdateLanguageAsync(Language language);
		Task<bool> RemoveLanguageAsync(string id);
	}
}
