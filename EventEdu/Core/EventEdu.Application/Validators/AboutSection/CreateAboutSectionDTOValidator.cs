using EventEdu.Application.DTOs.AboutSection;
using EventEdu.Application.DTOs.HeroSection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.AboutSection
{
    public class CreateAboutSectionDTOValidator : AbstractValidator<CreateAboutSectionDTO>
    {
        public CreateAboutSectionDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("About Section Title is required.")
                .Length(3, 100).WithMessage("About Section Title must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("About Section Description is required.")
                .Length(20, 10000).WithMessage("About Section Description must be between 20 and 500 characters.");

            RuleFor(x => x.LanguageId)
                .NotEqual(Guid.Empty).WithMessage("Language is required.");

            RuleFor(x => x.ImageFile)
                .NotEmpty().WithMessage("Image is required.");

        }
    }
}
