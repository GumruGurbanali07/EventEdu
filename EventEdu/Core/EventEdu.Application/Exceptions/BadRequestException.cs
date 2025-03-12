
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Exceptions
{
	public class BadRequestException : Exception
	{
		public BadRequestException() : base("Sorğu düzgün deyil. Zəhmət olmasa, məlumatları yoxlayın.")
		{
		}
		public BadRequestException(string? message) : base(message)
		{
		}

		public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}
