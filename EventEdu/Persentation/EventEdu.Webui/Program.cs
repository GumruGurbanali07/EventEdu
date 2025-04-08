using System.Globalization;
using EventEdu.Persistence;
using RequestLocalizationOptions = Microsoft.AspNetCore.Builder.RequestLocalizationOptions;
using Microsoft.AspNetCore.StaticFiles;
using System.Runtime.CompilerServices;
using EventEdu.Webui.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using EventEdu.Infrastructure;
using Microsoft.OpenApi.Models;
using EventEdu.Application;
using FluentValidation;
using System.Reflection;
using EventEdu.Application.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews().AddViewLocalization();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Sessiyanın bitmə müddəti
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.LoginPath = "/User/Login";
    });


builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizationFactory>();
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


builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IStringLocalizer, JsonStringLocalization>(); // Əlavə etdik


//builder.Services.AddIdentity<AppUser, AppRole>(options =>
//{
//    //options.SignIn.RequireConfirmedAccount = false;
//    //options.User.RequireUniqueEmail = false;


//    // Add services to the container.

//    options.Lockout.AllowedForNewUsers = true;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = false;
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
//    options.Lockout.MaxFailedAccessAttempts = 300;
//}).AddEntityFrameworkStores<AppDbContext>()
//  .AddDefaultTokenProviders();

builder.Services.AddResponseCompression(option =>
{
    option.EnableForHttps = true;
    option.Providers.Add<BrotliCompressionProvider>();
    option.Providers.Add<GzipCompressionProvider>();

});
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<BrotliCompressionProviderOptions>(option =>
{
    option.Level = CompressionLevel.SmallestSize;
});
builder.Services.Configure<GzipCompressionProviderOptions>(option =>
{
    option.Level = CompressionLevel.SmallestSize;
});
var app = builder.Build();

// mvc
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//mvc 
app.UseSession();


app.UseHttpsRedirection();

app.UseStaticFiles();

// var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
// app.UseRequestLocalization(locOptions!.Value);
//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture(new CultureInfo("az-AZ"))
//});

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("ru-RU"),
    new CultureInfo("az-AZ")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});


app.UseMiddleware<LocalizationMiddleware>();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
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