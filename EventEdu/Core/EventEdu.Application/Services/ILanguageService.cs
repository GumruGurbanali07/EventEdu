
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
		Task<List<LanguageGetDTO>> GetLanguagesAsync();
		Task<LanguageGetDTO> GetLanguageAsync(string isoCode);
		Task UpdateLanguageAsync(Guid id, UpdateLanguageDTO updateLanguageDTO);
		Task SoftDeleteLanguageAsync(Guid languageId);
		Task RestoreLanguageAsync(Guid languageId);
	}
}
