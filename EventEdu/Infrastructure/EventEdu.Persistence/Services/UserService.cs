using AutoMapper;
using Azure.Core;
using EventEdu.Application.DTOs.User;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using EventEdu.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EventEdu.Persistence.Services
{
    [Area("Admin")]
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, RoleManager<IdentityRole> roleManager, 
            IHostingEnvironment environment, IFileService fileService, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _environment = environment;
            _fileService = fileService;
            _context = context;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User already exists with this email." });
            }

            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." });
            }

            var user = _mapper.Map<AppUser>(registerDTO);
            user.Firstname = registerDTO.Firstname;
            user.Lastname = registerDTO.Lastname;
            user.EmailConfirmed = false;

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                return result;
            }

            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role 'Admin' does not exist." });
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            return result;
        }

        public async Task<SignInResult> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginDTO.Email, userLoginDTO.Password, userLoginDTO.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return SignInResult.Failed;
            }
            return result;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task VerifyEmail(ForgotPasswordDTO verifyEmailDTO)
        {

            var user = await _userManager.FindByNameAsync(verifyEmailDTO.Email);
            if (user == null)
            {
                throw new Exception("Something is wrong!");
            }
        }

    }
}
