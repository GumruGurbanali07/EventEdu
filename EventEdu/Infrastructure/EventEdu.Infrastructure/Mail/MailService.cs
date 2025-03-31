using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventEdu.Application.Services;

namespace EventEdu.Infrastructure.Mail
{
	public class MailService : IMailService
	{
		private readonly IConfiguration _configuration;

		public MailService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
		{
			await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
		}

		public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
		{
			MailMessage mail = new MailMessage();
			mail.IsBodyHtml = isBodyHtml;
			foreach (var to in tos)
			{
				mail.To.Add(to);
				mail.Subject = subject;
				mail.Body = body;
				mail.From = new(_configuration["Mail:UserName"], "Nalburla", System.Text.Encoding.UTF8);
			}


			SmtpClient smtp = new SmtpClient();

			smtp.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
			smtp.Port = 587;
			smtp.EnableSsl = true;
			smtp.Host = _configuration["Mail:Host"];
			await smtp.SendMailAsync(mail);

		}
	}
}