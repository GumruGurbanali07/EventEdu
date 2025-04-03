using EventEdu.Application.DTOs.Event;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.Event
{
	public class CreateEventDTOValidator : AbstractValidator<CreateEventDTO>
	{
		public CreateEventDTOValidator()
		{
			RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
			RuleFor(x => x.CategoriesId).NotEmpty().WithMessage("CategoryId is required.");
			RuleFor(x => x.StartDate).NotEmpty().WithMessage("StartDate is required.");
			RuleFor(x => x.EndDate).NotEmpty().WithMessage("EndDate is required.");
			RuleFor(x => x.LanguageId).NotEmpty().WithMessage("LanguageId is required.");
			RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");
			var allowedExtenstion = new[] { ".jpg", ".jpeg", ".png", ".gif" };
			RuleFor(x => x.FormFile)
				.Must(a => a.Length > 0).WithMessage("File cannot be empty")
				.Must(a => allowedExtenstion.Contains(Path.GetExtension(a.FileName).ToLower())).WithMessage("Only .jpg, .jpeg, .png, .gif formats are allowed ")
				.Must(a => a.Length <= 5 * 1024 * 1024).WithMessage("File  size must be less  than 5MB");


		}
	}
}
