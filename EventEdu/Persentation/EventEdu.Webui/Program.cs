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

var builder = WebApplication.CreateBuilder(args);


// builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews().AddViewLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Sessiyanın bitmə müddəti
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizationFactory>();
builder.Services.AddAutoMapper(typeof(AutoMapping));


builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());




// Add services to the container.
builder.Services.AddControllersWithViews();


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
app.UseRequestLocalization(new RequestLocalizationOptions
{
	DefaultRequestCulture = new RequestCulture(new CultureInfo("az-AZ"))
});

app.UseMiddleware<LocalizationMiddleware>();


app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();
app.MapStaticAssets();


app.MapAreaControllerRoute(
	name: "areas",
	areaName: "admin",
	pattern: "admin/{controller=Dashboards}/{action=Index}/{id?}"
);
app.MapControllerRoute(
	name: "eventCategory",
	pattern: "event/{categoryName}",
	defaults: new { controller = "Event", Action = "eventCategory" });
app.MapDefaultControllerRoute();
	


app.Run();