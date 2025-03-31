
using EventEdu.Application.DTOs.Language;
using EventEdu.Domain.Entities;

namespace EventEdu.Application.Services
{
	public interface ILanguageService
	{
		Task<Language> CreateAsync(CreateLanguageDTO languageDTO);
		Task<List<Language>> GetLanguagesAsync();
		Task<Language> GetLanguageById(Guid id);
		Task<LanguageGetDTO> GetLanguageAsync(string isoCode);
		Task UpdateLanguageAsync(Guid id, UpdateLanguageDTO updateLanguageDTO);
		Task SoftDeleteLanguageAsync(Guid languageId);
		Task RestoreLanguageAsync(Guid languageId);
	}
}
