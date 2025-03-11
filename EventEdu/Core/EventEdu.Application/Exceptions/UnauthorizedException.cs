using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Giriş icazəsi tələb olunur.")
        {
        }
        public UnauthorizedException(string? message) : base(message)
        {
        }

        public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}