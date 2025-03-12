using EventEdu.Application.DTOs.Speaker;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.Speaker
{
	public class CreateSpeakerDTOValidator : AbstractValidator<CreateSpeakerDTO>
	{
		public CreateSpeakerDTOValidator()
		{
			RuleFor(x => x.FullName)
				.NotEmpty().WithMessage("Full Name is required.")
				.Length(3, 100).WithMessage("Full Name must be between 3 and 100 characters.");

			RuleFor(x => x.Bio)
				.NotEmpty().WithMessage("Bio is required.")
				.MaximumLength(500).WithMessage("Bio must be less than 500 characters.");

			RuleFor(x => x.LanguageId)
				.NotEqual(Guid.Empty).WithMessage("Language ID is required.");

			RuleFor(x => x.ImageUrl)
				.NotEmpty().WithMessage("Image URL is required.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid Email format.");

			RuleFor(x => x.FacebookLink)
				.NotEmpty().WithMessage("Facebook Link is required.");

			RuleFor(x => x.TwitterLink)
				.NotEmpty().WithMessage("Twitter Link is required.");

			RuleFor(x => x.InstagramLink)
				.NotEmpty().WithMessage("Instagram Link is required.");
		}
	}
}
