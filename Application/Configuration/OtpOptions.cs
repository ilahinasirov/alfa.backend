using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class OtpOptions
    {
        public int OtpExpiryMinutes { get; set; }
        public int ResendOtpRequiredSeconds { get; set; }
    }
}
