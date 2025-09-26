using Shared.Models.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Portal.Auth
{
    public class AuthSessionInfo
    {
        public required string AccessToken { get; set; }
        public required string IdToken { get; set; }
        public required string TokenType { get; set; }
        public required LoginDetailDto LoginDetail { get; set; }
        public CertificateDto? Certificate { get; set; }
    }
}
