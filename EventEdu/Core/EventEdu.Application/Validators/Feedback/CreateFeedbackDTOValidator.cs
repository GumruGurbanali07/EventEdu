using FluentValidation;
using EventEdu.Application.DTOs.Feedback;

namespace EventEdu.Application.Validators
{
	public class CreateFeedbackDTOValidator : AbstractValidator<AddFeedBackDTO>
	{
		public CreateFeedbackDTOValidator()
		{
			RuleFor(x => x.Rating)
				.InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

			RuleFor(x => x.Comment)
				.NotEmpty().WithMessage("Comment is required.")
				.MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");

			RuleFor(x => x.EventId)
				.NotEmpty().WithMessage("EventId is required.");

			RuleFor(x => x.LanguageId)
				.NotEmpty().WithMessage("LanguageId is required.");
		}
	}
}
