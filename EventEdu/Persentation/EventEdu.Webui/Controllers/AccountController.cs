using Azure.Core;
using Azure;
using EventEdu.Application.DTOs.PersonalData;
using EventEdu.Application.Services;
using EventEdu.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using EventEdu.Persistence.Services;

public class AccountController : Controller
{
    private readonly IAccountServiceForPersonalData _accountService;
    private readonly AppDbContext _context;

    public AccountController(IAccountServiceForPersonalData accountService, AppDbContext context)
    {
        _accountService = accountService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Profile(Guid id)
    {
        var personalDataCookie = Request.Cookies["PersonalData"];

        if (personalDataCookie != null)
        {
            // Extract the ID from the cookie or session if necessary
            var personalDataFromCookie = personalDataCookie?.Split("|");

            //var PersonalIdForEdit = await _accountService.GetPersonalDatasById(id);

            //if (PersonalIdForEdit == null)
            //{
            //    return NotFound("Sponsor not found.");
            //}


            return RedirectToAction("EditProfile", new { id = id });
        }

        var personalDataFromCookieSplit = personalDataCookie?.Split("|");

        var model = new CreatePersonalDataDTO
        {
            Firstname = personalDataFromCookieSplit?[0],
            Lastname = personalDataFromCookieSplit?[1],
            Email = personalDataFromCookieSplit?[2],
            Birthday = personalDataFromCookieSplit?[3],
            Gender = personalDataFromCookieSplit?[4],
            PhoneNumber = personalDataFromCookieSplit?[5]
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Profile([FromForm] CreatePersonalDataDTO addPersonalData)
    {
        if (!ModelState.IsValid)
        {
            return View("Profile", addPersonalData);
        }

        Response.Cookies.Append("PersonalData", $"{addPersonalData.Firstname}|" +
            $"{addPersonalData.Lastname}|" +
            $"{addPersonalData.Email}|" +
            $"{addPersonalData.Birthday}|" +
            $"{addPersonalData.Gender}|" +
            $"{addPersonalData.PhoneNumber}|", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddYears(1),
                Secure = true,
                SameSite = SameSiteMode.None,
            });

        await _accountService.AddPersonalData(addPersonalData);

        TempData["SuccessMessage"] = "Your personal data has been saved successfully.";

        // You can optionally retrieve the ID here from the stored data or create it dynamically
        var id = Guid.NewGuid(); // Example, replace with actual ID retrieval logic

        return RedirectToAction("EditProfile", new { id = id });
    }

    [HttpGet]
    public async Task<IActionResult> EditProfile(Guid id)
    {
        var successMessage = TempData["SuccessMessage"]?.ToString();

        var personalDataCookie = Request.Cookies["PersonalData"];
        var personalDataFromCookie = personalDataCookie?.Split("|");

        var model = new CreatePersonalDataDTO
        {
            Id = id,
            Firstname = personalDataFromCookie?[0],
            Lastname = personalDataFromCookie?[1],
            Email = personalDataFromCookie?[2],
            Birthday = personalDataFromCookie?[3],
            Gender = personalDataFromCookie?[4],
            PhoneNumber = personalDataFromCookie?[5]
        };

        ViewBag.SuccessMessage = successMessage;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(Guid id, CreatePersonalDataDTO updatePersonalData)
    {
        if (!ModelState.IsValid)
        {
            return View(updatePersonalData);
        }

        try
        {
            await _accountService.EditPersonalData(updatePersonalData.Id, updatePersonalData);

            Response.Cookies.Append("PersonalData", $"{updatePersonalData.Firstname}|" +
                $"{updatePersonalData.Lastname}|" +
                $"{updatePersonalData.Email}|" +
                $"{updatePersonalData.Birthday}|" +
                $"{updatePersonalData.Gender}|" +
                $"{updatePersonalData.PhoneNumber}|", new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddYears(1),
                    Secure = true,
                    SameSite = SameSiteMode.None,
                });

            ViewBag.Message = "Profile updated successfully!";
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(updatePersonalData);
        }

        return View(updatePersonalData);
    }

    public IActionResult ParticipationHistory()
    {
        return View();
    }
}
