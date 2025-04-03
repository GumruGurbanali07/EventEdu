using AutoMapper;
using EventEdu.Application.DTOs.Category;
using EventEdu.Application.DTOs.Event;
using EventEdu.Application.DTOs.Language;
using EventEdu.Application.DTOs.Speaker;
using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.HeroSection;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.ViewModel;
using EventEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventEdu.Application.DTOs.Feedback;

namespace EventEdu.Application.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            //Sponsor
            CreateMap<Sponsor, CreateSponsorDTO>().ReverseMap();
            CreateMap<Sponsor, GetSponsorDTO>().ReverseMap();
            CreateMap<SponsorDetail, CreateSponsorDTO>().ReverseMap();
            CreateMap<SponsorDetail, GetSponsorDTO>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));

            //HeroSection
            CreateMap<HeroSection, CreateHeroSectionDTO>().ReverseMap();
            CreateMap<HeroSection, GetHeroSectionDTO>().ReverseMap();
            CreateMap<HeroSectionDetails, CreateHeroSectionDTO>().ReverseMap();
            CreateMap<HeroSectionDetails, GetHeroSectionDTO>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));

            //AboutSection
            CreateMap<AboutSection, CreateAboutSectionDTO>().ReverseMap();
            CreateMap<AboutSection, GetAboutSectionDTO>().ReverseMap();
            CreateMap<AboutSectionDetail, CreateAboutSectionDTO>().ReverseMap();
            CreateMap<AboutSectionDetail, GetAboutSectionDTO>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(4)));

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

            //Feedback
            CreateMap<FeedBack,AddFeedBackDTO >().ReverseMap();
            CreateMap<FeedBack, UpdateFeedbackDTO>().ReverseMap();
            CreateMap<FeedBack, GetFeedbackDTO>().ReverseMap();
            CreateMap<FeedBackDetail, AddFeedBackDTO>().ReverseMap();
            CreateMap<FeedBackDetail, GetFeedbackDTO>().ReverseMap();
			CreateMap<UpdateFeedbackDTO, FeedBackDetail>()
	       .ForMember(dest => dest.Id, opt => opt.Ignore())
	       .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
	       .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));







            //UserPersonalData
            CreateMap<CreatePersonalDataDTO, PersonalData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) // Auto-generate Id
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.Birthday) ? (DateTime?)null : DateTime.Parse(src.Birthday))) // Convert string to DateTime
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender));
        }
    }


}