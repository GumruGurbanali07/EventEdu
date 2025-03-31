using AutoMapper;
using EventEdu.Application.DTOs.PersonalData;
using EventEdu.Application.DTOs.PersonalData;
using EventEdu.Application.Repository;
using EventEdu.Application.Services;
using EventEdu.Domain.Entities.Identity;
using EventEdu.Persistence.Context;
using EventEdu.Persistence.Extensions;
using EventEdu.Persistence.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Services
{
    public class AccountService : IAccountServiceForPersonalData
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreatePersonalDataDTO> _createPersonalDataValidator;
        private readonly IMapper _mapper;

        public AccountService(AppDbContext context, IValidator<CreatePersonalDataDTO> createPersonalDataValidator, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _createPersonalDataValidator = createPersonalDataValidator;
        }

        public async Task AddPersonalData(CreatePersonalDataDTO addPersonalData)
        {

            var validationResult = await _createPersonalDataValidator.ValidateAsync(addPersonalData);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var personalData = _mapper.Map<PersonalData>(addPersonalData);
            personalData.Id = Guid.NewGuid();
            await _context.AddAsync(personalData);
            _context.SaveChanges();

        }

        public async Task<GetPersonalDataDTO> GetPersonalDatasById(Guid id)
        {
            var personalData = await _context.PersonalDatas
                .Where(s => s.Id == id)
                .Select(s => new GetPersonalDataDTO
                {
                    Id = s.Id,
                    Email = s.Email,
                    Firstname = s.Firstname,
                    Lastname = s.Lastname,
                    Gender = s.Gender,
                    Birthday = s.Birthday,
                    PhoneNumber = s.PhoneNumber
                })
                .FirstOrDefaultAsync();

            return personalData;
        }


        public async Task EditPersonalData(Guid id, CreatePersonalDataDTO updatePersonalDataDTO)
        {
            var PersonalData = _context.PersonalDatas
            .FirstOrDefault(s => s.Id == id
          );

            if (PersonalData == null)
            {
                throw new Exception("PersonalData not found.");
            }

            var validationResult = await _createPersonalDataValidator.ValidateAsync(updatePersonalDataDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            PersonalData.Id = updatePersonalDataDTO.Id;
            PersonalData.Email = updatePersonalDataDTO.Email;
            PersonalData.Firstname = updatePersonalDataDTO.Firstname;
            PersonalData.Lastname = updatePersonalDataDTO.Lastname;
            PersonalData.Gender = updatePersonalDataDTO.Gender;
            PersonalData.Birthday = updatePersonalDataDTO.Birthday;
            PersonalData.PhoneNumber = updatePersonalDataDTO.PhoneNumber;

            _context.PersonalDatas.Update(PersonalData);
            _context.SaveChanges();
        }




    }
}
