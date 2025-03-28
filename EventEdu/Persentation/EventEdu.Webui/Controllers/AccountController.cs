using EventEdu.Application.DTOs.PersonalData;
using EventEdu.Application.DTOs.Sponsor;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EventEdu.Webui.Controllers
{
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
            try
            {
                CreatePersonalDataDTO personalData = null;
                var cookieData = Request.Cookies["PersonalData"];

                if (!string.IsNullOrEmpty(cookieData))
                {
                    var dataParts = cookieData.Split('|');

                    if (dataParts.Length >= 6)  // Ensure all parts exist
                    {
                        personalData = new CreatePersonalDataDTO
                        {
                            Firstname = dataParts[0],
                            Lastname = dataParts[1],
                            Email = dataParts[2],
                            Birthday = dataParts[3],
                            Gender = dataParts[4],
                            PhoneNumber = dataParts[5]
                        };
                    }
                }
                else
                {
                    // Fetch data from the database if cookie is empty
                    var fetchedData = await _accountService.GetPersonalDatasById(id);

                    if (fetchedData != null)
                    {
                        personalData = new CreatePersonalDataDTO
                        {
                            Id = fetchedData.Id,
                            Email = fetchedData.Email,
                            Firstname = fetchedData.Firstname,
                            Lastname = fetchedData.Lastname,
                            Gender = fetchedData.Gender,
                            Birthday = fetchedData.Birthday,
                            PhoneNumber = fetchedData.PhoneNumber
                        };
                    }
                    else
                    {
                        return View("EditProfile");  // No data found, redirect to EditProfile
                    }
                }

                return View("Profile", personalData);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
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

            return RedirectToAction("Profile", new { id = addPersonalData.Id });
        }



        [HttpGet]
        public async Task<IActionResult> EditProfile(Guid id)
        {
            try
            {
                var personalData = await _accountService.GetPersonalDatasById(id);

                if (personalData == null)
                {
                    return View(new CreatePersonalDataDTO());
                }

                var updatePersonalData = new CreatePersonalDataDTO
                {
                    Id = personalData.Id,
                    Email = personalData.Email,
                    Firstname = personalData.Firstname,
                    Lastname = personalData.Lastname,
                    Gender = personalData.Gender,
                    Birthday = personalData.Birthday,
                    PhoneNumber = personalData.PhoneNumber
                };



                return View(updatePersonalData);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
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
}
