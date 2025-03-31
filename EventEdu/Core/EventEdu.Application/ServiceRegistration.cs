using EventEdu.Application.Validators;
using EventEdu.Application.Validators.Category;
using EventEdu.Application.Validators.Event;
using EventEdu.Application.Validators.Language;
using EventEdu.Application.Validators.Speaker;
using EventEdu.Application.Validators.Subscription;
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

			services.AddValidatorsFromAssemblyContaining<CreateCategoryDTOValidator>();
			services.AddValidatorsFromAssemblyContaining<UpdateCategoryDTOValidator>();

			services.AddValidatorsFromAssemblyContaining<CreateSpeakerDTOValidator>();
			services.AddValidatorsFromAssemblyContaining<UpdateSpeakerDTOValidator>();

			services.AddValidatorsFromAssemblyContaining<CreateEventDTOValidator>();
			services.AddValidatorsFromAssemblyContaining<UpdateEventDTOValidator>();

			services.AddValidatorsFromAssemblyContaining<SubscribeDTOValidator>();

			services.AddValidatorsFromAssemblyContaining<CreateFeedbackDTOValidator>();
			services.AddValidatorsFromAssemblyContaining<UpdateFeedbackDTOValidator>();
  



		}
	}
}


