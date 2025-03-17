
using EventEdu.Application.DTOs.Language;
using EventEdu.Domain.Entities;

namespace EventEdu.Application.Services
{
	public interface ILanguageService
	{
		Task<Language> CreateAsync(CreateLanguageDTO languageDTO);
		Task<List<LanguageGetDTO>> GetLanguagesAsync();
		Task<LanguageGetDTO> GetLanguageAsync(string IsoCode);
		Task UpdateLanguageAsync(Guid id, UpdateLanguageDTO updateLanguageDTO);
		Task SoftDeleteLanguageAsync(Guid languageId);
		Task RestoreLanguageAsync(Guid languageId);
	}
}
