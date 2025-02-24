using EventEdu.Persistence;
using RequestLocalizationOptions = Microsoft.AspNetCore.Builder.RequestLocalizationOptions;
using Microsoft.AspNetCore.StaticFiles;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

 
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews().AddViewLocalization();


 
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

app.UseHttpsRedirection();
 
app.UseStaticFiles();

var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions!.Value);

  
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


app.Run();