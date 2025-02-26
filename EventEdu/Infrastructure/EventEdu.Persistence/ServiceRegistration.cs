using System.Globalization;
using System.Reflection;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Repository;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventEdu.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
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
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            }).AddEntityFrameworkStores<AppDbContext>();
            //Services
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ILanguageService, LanguageService>();
			services.AddScoped<ICategoryService, CategoryService>();

            //Repositories
			services.AddSingleton<StringLocalizerService>();
            services.AddScoped<ILanguageReadRepository, LanguageReadRepository>();
            services.AddScoped<ILanguageWriteRepository, LanguageWriteRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            services.AddScoped<ICategoryDetailReadRepository, CategoryDetailReadRepository>();
            services.AddScoped<ICategoryDetailWriteRepository, CategoryDetailWriteRepository>();


            
        }
    }
}