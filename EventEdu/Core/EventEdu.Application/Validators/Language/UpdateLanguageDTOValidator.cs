using EventEdu.Application.DTOs.Language;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.Language
{
    public class UpdateLanguageDTOValidator:AbstractValidator<UpdateLanguageDTO>
    {
		public UpdateLanguageDTOValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Dil adı boş olmamalıdır.")
				.Length(2, 50).WithMessage("Dil adı 2 ilə 50 simvol arasında olmalıdır.");

			RuleFor(x => x.IsoCode)
				.NotEmpty().WithMessage("ISO kodu boş olmamalıdır.")
				.Length(2, 5).WithMessage("ISO kodu 2 ilə 5 simvol arasında olmalıdır.");

			RuleFor(x => x.ImagePath)
				.NotEmpty().WithMessage("Şəkil yolu boş olmamalıdır.");
		}
	}
}
