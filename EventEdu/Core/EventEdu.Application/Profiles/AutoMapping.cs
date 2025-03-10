using AutoMapper;
using EventEdu.Application.DTOs.Category;
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
	public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			CreateMap<Language, CreateLanguageDTO>().ReverseMap();
			CreateMap<Language, LanguageGetDTO>().ReverseMap();
			CreateMap<UpdateLanguageDTO, Language>()
	        .ForMember(dest => dest.Id, opt => opt.Ignore()) 
	        .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) 
	        .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow)); 

			CreateMap<Category, CreateCategoryDTO>().ReverseMap();
			CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
			CreateMap<Category, GetCategoryDTO>().ReverseMap();

			CreateMap<CategoryDetail, GetCategoryDTO>().ReverseMap();
			CreateMap<CategoryDetail, CreateCategoryDTO>().ReverseMap();

			CreateMap<UpdateCategoryDTO, CategoryDetail>()
	       .ForMember(dest => dest.Id, opt => opt.Ignore())
	       .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
	       .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

		}
	}
}
