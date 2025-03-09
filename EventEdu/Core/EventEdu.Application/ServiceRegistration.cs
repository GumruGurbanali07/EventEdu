using EventEdu.Application.Validators.Language;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application
{
   public static class   ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
			services.AddValidatorsFromAssemblyContaining<CreateLanguageDTOValidator>();
			services.AddValidatorsFromAssemblyContaining<UpdateLanguageDTOValidator>();


		}
	}
}
