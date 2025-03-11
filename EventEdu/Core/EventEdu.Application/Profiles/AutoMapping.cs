using AutoMapper;
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

namespace EventEdu.Application.Profiles
{
	public class AutoMapping:Profile
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

        }
    }
}
