using EventEdu.Application.DTOs.Subscription;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Validators.Subscription
{
	public class SubscribeDTOValidator : AbstractValidator<SubscribeDTO>
	{
		public SubscribeDTOValidator()
		{
			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("Ad boş ola bilməz!")
				.MinimumLength(2).WithMessage("Ad ən az 2 simvol olmalıdır!")
				.MaximumLength(50).WithMessage("Ad ən çox 50 simvol ola bilər!");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Soyad boş ola bilməz!")
				.MinimumLength(2).WithMessage("Soyad ən az 2 simvol olmalıdır!")
				.MaximumLength(50).WithMessage("Soyad ən çox 50 simvol ola bilər!");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email boş ola bilməz!")
				.EmailAddress().WithMessage("Düzgün email formatı daxil edin!");
		}
	}
}
