using EventEdu.Application.DTOs.PersonalData;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.AccountForUserPersonalData
{
    public class CreatePersonalDataValidator : AbstractValidator<CreatePersonalDataDTO>
    {
        public CreatePersonalDataValidator()
        {
            RuleFor(x => x.Firstname)
                .Length(3, 100).WithMessage("Firstname must be between 3 and 20 characters.");

            RuleFor(x => x.Lastname)
              .Length(3, 100).WithMessage("Lastname must be between 3 and 20 characters.");

            RuleFor(x => x.Email)
                 .EmailAddress().WithMessage("Invalid Email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Matches(@"^(\+994\s?0?|0)(10|12|50|51|55|70|77)\s?\d{3}\s?\d{2}\s?\d{2}$")
                .WithMessage("Invalid phone number format. Examples: +994501234567, 0501234567, 501234567, +994 50 123 45 67, +994 050 123 45 67");


        }
    }
}
