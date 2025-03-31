using EventEdu.Application.Services;
using EventEdu.Infrastructure.Mail;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Infrastructure
{
	public static class ServiceRegistration
	{
		public static void AddInfrastructureServices(this IServiceCollection services)
		{			
			services.AddTransient<IMailService, MailService>();
		}
	}
}
