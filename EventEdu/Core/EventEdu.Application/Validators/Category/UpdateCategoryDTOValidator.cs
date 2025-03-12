using EventEdu.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.Category
{
	public class UpdateCategoryDTOValidator : AbstractValidator<UpdateCategoryDTO>
	{
		public UpdateCategoryDTOValidator()
		{
			RuleFor(x => x.CategoryName)
				.NotEmpty().WithMessage("Category Name is required.")
				.Length(3, 100).WithMessage("Category Name must be between 3 and 100 characters.");

			RuleFor(x => x.LanguageId)
				.NotEqual(Guid.Empty).WithMessage("Language ID is required.");
		}
	}
}
