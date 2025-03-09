using AutoMapper;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Profiles
{
	public class AutoMapping:Profile
	{
		public AutoMapping()
		{
			CreateMap<Language, CreateLanguageDTO>().ReverseMap();
			CreateMap<Language, LanguageGetDTO>().ReverseMap();
			CreateMap<Language, UpdateLanguageDTO>().ReverseMap();
		}
	}
}
