using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Portal.Auth
{
    public class OtpInfoResponse
    {
        public DateTime OtpCreatedDate { get; set; }
        public DateTime OtpExpiredDate { get; set; }
        public int OtpExpiryMinutes { get; set; }
    }
}
