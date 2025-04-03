using EventEdu.Application.Services;
using EventEdu.Webui.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EventEdu.Webui.Controllers;

public class AboutController : Controller
{
    private readonly IAboutSectionService _aboutSectionService;

    public AboutController(IAboutSectionService aboutSectionService)
    {
        _aboutSectionService = aboutSectionService;
    }
    public async Task<IActionResult> IndexAsync()
    {
        var aboutSection = await _aboutSectionService.GetAllAboutSectionsAsync();
        var viewModel = new HomeIndexVM
        {
            AboutSection = aboutSection,
        };

        return View(viewModel);
    }
}