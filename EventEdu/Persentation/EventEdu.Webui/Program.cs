using System.Globalization;
using EventEdu.Persistence;
using RequestLocalizationOptions = Microsoft.AspNetCore.Builder.RequestLocalizationOptions;
using EventEdu.Webui.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllersWithViews().AddViewLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizationFactory>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // For GDPR compliance
});

 
builder.Services.AddPersistenceServices(builder.Configuration);
// Add services to the container.
builder.Services.AddControllersWithViews();

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

// var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
// app.UseRequestLocalization(locOptions!.Value);
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("az-AZ"))
});

app.UseMiddleware<LocalizationMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


app.Run();