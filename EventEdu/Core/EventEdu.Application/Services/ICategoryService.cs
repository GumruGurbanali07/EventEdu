using EventEdu.Application.DTOs.Category;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface ICategoryService
    {
		Task<List<CategoryDetail>> GetCategoriesByLanguageAsync(string isoCode);
		Task AddCategoryWithLanguageAsync(string categoryName, Guid languageId);
	}
}
