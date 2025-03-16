using EventEdu.Application.DTOs.HeroSection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.HeroSection
{
    public class CreateHeroSectionDTOValidator  : AbstractValidator<CreateHeroSectionDTO>
    {
        public CreateHeroSectionDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Hero Section Title is required.")
                .Length(3, 100).WithMessage("Hero Section Title must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Hero Section Description is required.")
                .Length(20, 500).WithMessage("Hero Section Description must be between 20 and 500 characters.");

            RuleFor(x => x.LanguageId)
                .NotEqual(Guid.Empty).WithMessage("Language ID is required.");

            RuleFor(x => x.ImageFile)
                .NotEmpty().WithMessage("Image is required.");

        }
    }
}
