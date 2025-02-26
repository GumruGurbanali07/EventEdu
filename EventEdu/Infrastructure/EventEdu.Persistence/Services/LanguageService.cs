using AutoMapper;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
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

		public LanguageService(ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository)
		{
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
		}

		public async Task<bool> AddLanguageAsync(LanguageAddDTO languageAddDTO)
		{
			var existLang = await _languageReadRepository.GetByIsoCodeAsync(languageAddDTO.IsoCode);
			if (existLang != null)
			{
				throw new Exception("Language is already exist!");
			}

			var language = new Language
			{
				Name = languageAddDTO.Name,
				IsoCode = languageAddDTO.IsoCode,
				ImagePath = languageAddDTO.ImagePath
			};

			var result = await _languageWriteRepository.AddAsync(language);
			if (result)
			{
				await _languageWriteRepository.SaveChangeAsync();
			}
			return result;
		}

		public Task<List<Language>> GetAllLanguagesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Language?> GetLanguageByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveLanguageAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateLanguageAsync(Language language)
		{
			throw new NotImplementedException();
		}
	}
}