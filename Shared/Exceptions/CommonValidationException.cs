using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public class CommonValidationException : BaseException
    {
        public CommonValidationException()
        {
        }

        public CommonValidationException(string? message) : base(message)
        {
        }
    }
}
