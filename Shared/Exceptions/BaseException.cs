using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    [Serializable]
    public class BaseException : ApplicationException
    {
        public int ErrorCode { get; }
        protected BaseException()
        {
        }

        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, int errorCode)
           : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
