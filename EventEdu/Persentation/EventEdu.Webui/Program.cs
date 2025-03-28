
using System.Globalization;
using EventEdu.Persistence;
using RequestLocalizationOptions = Microsoft.AspNetCore.Builder.RequestLocalizationOptions;
using EventEdu.Webui.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using EventEdu.Application.Services;
using EventEdu.Persistence.Services;
using EventEdu.Application.Profiles;
using EventEdu.Application.Validators.Sponsor;
using FluentValidation;
using EventEdu.Application.Validators.HeroSection;
using FluentValidation.AspNetCore;
using EventEdu.Application.Validators.AboutSection;
using EventEdu.Application.Validators.Language;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using EventEdu.Application.Validators.AccountForUserPersonalData;
//using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddViewLocalization().AddFluentValidation(fv =>
    fv.RegisterValidatorsFromAssemblyContaining<CreateLanguageDTOValidator>()
    .RegisterValidatorsFromAssemblyContaining<UpdateLanguageDTOValidator>()
    .RegisterValidatorsFromAssemblyContaining<CreateSponsorDTOValidator>()
     .RegisterValidatorsFromAssemblyContaining<CreateHeroSectionDTOValidator>()
      .RegisterValidatorsFromAssemblyContaining<CreateAboutSectionDTOValidator>()
     .RegisterValidatorsFromAssemblyContaining<CreatePersonalDataValidator>());
//.RegisterValidatorsFromAssemblyContaining<CreateUserDTOValidator>());


builder.Services.AddDistributedMemoryCache();
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizationFactory>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.Zero;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // For GDPR compliance
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.Expiration = TimeSpan.FromDays(7);
        options.Cookie.IsEssential = true;
    });



builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AutoMapping));


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    //options.SignIn.RequireConfirmedAccount = false;
    //options.User.RequireUniqueEmail = false;

    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;

    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
    options.Lockout.MaxFailedAccessAttempts = 300;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

//Swagger services
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(c =>
//{
//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventEdu API", Version = "v1" });
//});
//Swagger services



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("az-AZ"))
});

app.UseMiddleware<LocalizationMiddleware>();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//// Enable Swagger middleware
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventEdu API v1");
//});
//// Enable Swagger middleware


app.MapStaticAssets();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=User}/{action=Login}/{id?}"
          );

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


app.Run();
