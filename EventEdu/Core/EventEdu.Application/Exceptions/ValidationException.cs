
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Exceptions
{
	public class ValidationException : Exception
	{
		public ValidationException() : base("Doğrulama səhv oldu, məlumat düzgün deyil.")
		{
		}
		public ValidationException(string? message) : base(message)
		{
		}

		public ValidationException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}
