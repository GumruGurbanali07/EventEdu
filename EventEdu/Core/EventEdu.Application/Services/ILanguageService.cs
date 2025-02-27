using EventEdu.Application.DTOs.Language;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
	public interface ILanguageService
	{
		Task<LanguageGetDto> GetLanguageAsync(Expression<Func<Language, bool>> predicate);
		List<LanguageGetDto> GetAll();
		Task<LanguageGetDto> GetSelectedLanguageAsync();

	}
}
