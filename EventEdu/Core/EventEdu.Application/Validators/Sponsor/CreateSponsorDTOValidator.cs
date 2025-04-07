using EventEdu.Application.DTOs.Sponsor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.Sponsor
{
    public class CreateSponsorDTOValidator : AbstractValidator<CreateSponsorDTO>
    {
        public CreateSponsorDTOValidator()
        {
            RuleFor(x => x.SponsorName)
                .NotEmpty().WithMessage("Sponsor Name is required.")
                .Length(3, 100).WithMessage("Sponsor Name must be between 3 and 100 characters.");

            RuleFor(x => x.SponsorDescription)
               .NotEmpty().WithMessage("Sponsor Description is required.")
                .Length(20, 500).WithMessage("Sponsor Description must be between 20 and 500 characters.");

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required.")
                 .EmailAddress().WithMessage("Invalid Email format.");

			RuleFor(x => x.Website)
	.NotEmpty().WithMessage("Website is required.")
	.Matches(@"^(https?:\/\/)?(www\.)?[a-zA-Z0-9-]+(\.[a-zA-Z]{2,}){1,2}(\/[^\s]*)?$")
	.WithMessage("Invalid Website format.");



			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Phone Number is required.")
				.Matches(@"^(\+994\s?|0)(10|50|51|55|70|77)\s?\d{3}\s?\d{2}\s?\d{2}$|^(\+994\s?|0)12\s?\d{3}\s?\d{2}\s?\d{2}$")
				.WithMessage("Invalid phone number format. Examples: +994501234567, 0501234567, 501234567, +994 50 123 45 67, +994 12 493 00 91");


			RuleFor(x => x.LanguageId)
                .NotEqual(Guid.Empty).WithMessage("Language  is required.");

            RuleFor(x => x.ImageFile)
                .NotEmpty().WithMessage("Image is required.");

            //RuleFor(x => x.ImageFile)
            //    .NotNull().WithMessage("Image file is required.")
            //    .Must(file => file.CheckFileType("image")).WithMessage("Invalid file type. Please upload an image.")
            //    .Must(file => file.CheckFileSize(10)).WithMessage("File size is too large. Maximum allowed size is 10MB.");

        }
    }
}
