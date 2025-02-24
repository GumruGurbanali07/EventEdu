using AutoMapper;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
	public class LanguageService : ILanguageService
	{
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		private readonly IMapper _mapper;
		public LanguageService(ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IMapper mapper)
		{
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
			_mapper = mapper;
		}

		public async Task<LanguageViewModel> GetLanguageAsync(string isoCode)
		{
			var languages = _languageReadRepository.GetAll();

			var language = languages.FirstOrDefault(x => x.IsoCode.ToLower() == isoCode.ToLower());

			return  _mapper.Map<LanguageViewModel>(language);
		}

		public async Task<List<LanguageViewModel>> GetLanguagesAsync()
		{
			var languages =await _languageReadRepository.GetAll().ToListAsync();
			var languageViewModels=_mapper.Map<List<LanguageViewModel>>(languages);
			return languageViewModels;

 		}
	}
}
