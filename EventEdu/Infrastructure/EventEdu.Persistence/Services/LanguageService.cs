using AutoMapper;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
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
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		public LanguageService(ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IMapper mapper, AppDbContext context)
		{
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
			_mapper = mapper;
			_context = context;
		}

		public async Task<Language> CreateAsync(CreateLanguageDTO languageDTO)
		{
			if (_context.Languages.Any(x => x.IsoCode == languageDTO.IsoCode))
			{
				throw new InvalidOperationException("Bu ISO kodlu dil artıq mövcuddur.");
			}
			var newLang = new Language
			{
				IsoCode = languageDTO.IsoCode,
				ImagePath = languageDTO.ImagePath,
				Name = languageDTO.Name
			};
			await _languageWriteRepository.AddAsync(newLang);
			await _languageWriteRepository.SaveChangeAsync();
			return newLang;
		}

		public async Task AddLanguageAsync(LanguageViewModel languageViewModel)
		{
			var language = _mapper.Map<Language>(languageViewModel);
			await _languageWriteRepository.AddAsync(language);
			await _languageWriteRepository.SaveChangeAsync();
		}

		

		public async Task<LanguageViewModel> GetLanguageAsync(string isoCode)
		{
			var languages = _languageReadRepository.GetAll();

			var language = languages.FirstOrDefault(x => x.IsoCode.ToLower() == isoCode.ToLower());

			return _mapper.Map<LanguageViewModel>(language);
		}

		public async Task<List<LanguageViewModel>> GetLanguagesAsync()
		{
			var languages = await _languageReadRepository.GetAll().ToListAsync();
			var languageViewModels = _mapper.Map<List<LanguageViewModel>>(languages);
			return languageViewModels;
		}
	}
}