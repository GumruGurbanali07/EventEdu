
using System.Globalization;
using System.Reflection;
using EventEdu.Application.Repositor;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Repository;
using EventEdu.Persistence.Services;
using EventEdu.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventEdu.Persistence
{
	public static class ServiceRegistration
	{
		public static async void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<RequestLocalizationOptions>(options =>
			{
				var suportedCultures = new List<CultureInfo>
				{
					new CultureInfo("en-US"),
					new CultureInfo("az")
				};
				options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
				options.SupportedCultures = suportedCultures;
				options.SupportedUICultures = suportedCultures;
			});

			services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

      //      services.AddIdentity<AppUser, IdentityRole>(options =>
      //      {
      //          //options.SignIn.RequireConfirmedAccount = false;
      //          //options.User.RequireUniqueEmail = false;

      //          options.User.RequireUniqueEmail = true;

      //          options.Password.RequireDigit = true;
      //          options.Password.RequireLowercase = true;
      //          options.Password.RequiredLength = 6;

      //          options.Lockout.AllowedForNewUsers = true;
      //          options.Password.RequireNonAlphanumeric = false;
      //          options.Password.RequireUppercase = false;
      //          options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
      //          options.Lockout.MaxFailedAccessAttempts = 300;
      //      }).AddEntityFrameworkStores<AppDbContext>()
      //.AddDefaultTokenProviders();

			

				//Services
				//services.AddAutoMapper(Assembly.GetExecutingAssembly());
				services.AddScoped<IFileService, FileService>();
            services.AddScoped<ISponsorService, SponsorService>();
            services.AddScoped<IHeroSectionService, HeroSectionService>();
            services.AddScoped<IAboutSectionService, AboutSectionService>();
            services.AddScoped<IEventSponsorService, EventSponsorService>();
            services.AddScoped<IEventSpeakerService, EventSpeakerService>();
            services.AddScoped<ILanguageService, LanguageService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<ISpeakerService, SpeakerService>();
			services.AddScoped<ISponsorService, SponsorService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IEventService, EventService>();
			services.AddScoped<ISubscriptionService, SubscriptionService>();
			services.AddScoped<IFeedbackService, FeedbackService>();
			services.AddTransient<IFileService, FileService>();

            //Repositories
            services.AddSingleton<StringLocalizerService>();

			services.AddScoped<ILanguageReadRepository, LanguageReadRepository>();
			services.AddScoped<ILanguageWriteRepository, LanguageWriteRepository>();

			services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
			services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
			services.AddScoped<ICategoryDetailReadRepository, CategoryDetailReadRepository>();
			services.AddScoped<ICategoryDetailWriteRepository, CategoryDetailWriteRepository>();

			services.AddScoped<ISubscriptionReadRepository, SubscriptionReadRepository>();
			services.AddScoped<ISubscriptionWriteRepository, SubscriptionWriteRepository>();
			services.AddScoped<ISubsEventReadRepository, SubsEventReadRepository>();
			services.AddScoped<ISubsEventWriteRepository, SubsEventWriteRepository>();

			services.AddScoped<IEventReadRepository, EventReadRepository>();
			services.AddScoped<IEventWriteRepository, EventWriteRepository>();
			services.AddScoped<IEventDetailReadRepository, EventDetailReadRepository>();
			services.AddScoped<IEventDetailWriteRepository, EventDetailWriteRepository>();
			services.AddScoped<IEventSpeakerReadRepository, EventSpeakerReadRepository>();
			services.AddScoped<IEventSpeakerWriteRepository, EventSpeakerWriteRepository>();
			services.AddScoped<IEventSponsorReadRepository, EventSponsorReadRepository>();
			services.AddScoped<IEventSponsorWriteRepository, EventSponsorWriteRepository>();

			services.AddScoped<IFeedbackReadRepository, FeedBackReadRepository>();
			services.AddScoped<IFeedbackWriteRepository, FeedBackWriteRepository>();
			services.AddScoped<IFeedBackDetailReadRepository, FeedBackDetailReadRepository>();
			services.AddScoped<IFeedBackDetailWriteRepository, FeedBackDetailWriteRepository>();

			

			services.AddScoped<ISpeakerReadRepository, SpeakerReadRepository>();
			services.AddScoped<ISpeakerWriteRepository, SpeakerWriteRepository>();
			services.AddScoped<ISpeakerDetailReadRepository, SpeakerDetailReadRepository>();
			services.AddScoped<ISpeakerDetailWriteRepository, SpeakerDetailWriteRepository>();

			services.AddScoped<ISponsorReadRepository, SponsorReadRepository>();
			services.AddScoped<ISponsorWriteRepository, SponsorWriteRepository>();
			services.AddScoped<ISponsorDetailReadRepository, SponsorDetailReadRepository>();
			services.AddScoped<ISponsorDetailWriteRepository, SponsorDetailWriteRepository>();
			

			services.AddScoped<IHeroSectionReadRepository, HeroSectionReadRepository>();
			services.AddScoped<IHeroSectionWriteRepository, HeroSectionWriteRepository>();
			services.AddScoped<IHeroSectionDetailReadRepository, HeroSectionDetailReadRepository>();
			services.AddScoped<IHeroSectionDetailWriteRepository, HeroSectionDetailWriteRepository>();

			services.AddScoped<IAboutSectionReadRepository, AboutSectionReadRepository>();
			services.AddScoped<IAboutSectionWriteRepository, AboutSectionWriteRepository>();
			services.AddScoped<IAboutSectionDetailReadRepository, AboutSectionDetailReadRepository>();
			services.AddScoped<IAboutSectionDetailWriteRepository, AboutSectionDetailWriteRepository>();


        }
	}
}
