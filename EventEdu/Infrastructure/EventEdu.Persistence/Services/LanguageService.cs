using AutoMapper;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
	public class LanguageService : ILanguageService
	{
		private readonly ILanguageReadRepository _languageReadRepository;
		private readonly ILanguageWriteRepository _languageWriteRepository;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IMapper _mapper;

		public LanguageService(ILanguageReadRepository languageReadRepository, ILanguageWriteRepository languageWriteRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
		{
			_languageReadRepository = languageReadRepository;
			_languageWriteRepository = languageWriteRepository;
			_mapper = mapper;
			_contextAccessor = contextAccessor;
		}

		public List<LanguageGetDto> GetAll()
		{
			var languages = _languageReadRepository.GetAll();
			var dtos = _mapper.Map<List<LanguageGetDto>>(languages);
			return dtos;
		}

		public async Task<LanguageGetDto> GetLanguageAsync(Expression<Func<Language, bool>> predicate)
		{
			var language = await _languageReadRepository.GetAsync(predicate);
			var dto = _mapper.Map<LanguageGetDto>(language);
			return dto;
		}

		public async Task<LanguageGetDto> GetSelectedLanguageAsync()
		{
			var culture = _contextAccessor.HttpContext?.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
			var isoCode = culture?.Substring(culture.LastIndexOf('=') + 1) ?? "az";
			var selectedLanguage = await _languageReadRepository.GetAsync(x => x.IsoCode == isoCode);
			return _mapper.Map<LanguageGetDto>(selectedLanguage);
		}
	}
}