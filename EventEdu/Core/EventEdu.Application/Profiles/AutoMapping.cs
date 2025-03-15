using AutoMapper;
using EventEdu.Application.DTOs.Category;
using EventEdu.Application.DTOs.Event;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.DTOs.Speaker;
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
			//Language
			CreateMap<Language, CreateLanguageDTO>().ReverseMap();
			CreateMap<Language, LanguageGetDTO>().ReverseMap();
			CreateMap<UpdateLanguageDTO, Language>()
	        .ForMember(dest => dest.Id, opt => opt.Ignore()) 
	        .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) 
	        .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow)); 

			//Category
			CreateMap<Category, CreateCategoryDTO>().ReverseMap();
			CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
			CreateMap<Category, GetCategoryDTO>().ReverseMap();
			CreateMap<CategoryDetail, GetCategoryDTO>().ReverseMap();
			CreateMap<CategoryDetail, CreateCategoryDTO>().ReverseMap();
			CreateMap<UpdateCategoryDTO, CategoryDetail>()
	       .ForMember(dest => dest.Id, opt => opt.Ignore())
	       .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
	       .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

			//Speaker
			CreateMap<Speaker, CreateSpeakerDTO>().ReverseMap();
			CreateMap<Speaker, UpdateSpeakerDTO>().ReverseMap();
			CreateMap<Speaker, GetSpeakerDTO>().ReverseMap();
			CreateMap<SpeakerDetail, GetSpeakerDTO>().ReverseMap();
			CreateMap<SpeakerDetail, CreateSpeakerDTO>().ReverseMap();
			CreateMap<UpdateSpeakerDTO,SpeakerDetail>()
		   .ForMember(dest => dest.Id, opt => opt.Ignore())
		   .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
		   .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

			//Event
			CreateMap<Event, CreateEventDTO>().ReverseMap();
			CreateMap<Event, UpdateEventDTO>().ReverseMap();
			CreateMap<Event, GetEventDTO>().ReverseMap();
			CreateMap<EventDetail, GetEventDTO>().ReverseMap();
			CreateMap<EventDetail, CreateEventDTO>().ReverseMap();
			CreateMap<UpdateEventDTO, EventDetail>()
		    .ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
			.ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));


		}
	}
}
