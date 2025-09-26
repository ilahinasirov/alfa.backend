using Shared.Models.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityAccessTokenDto> GetAccessTokenAsync(string code, string redirectUrl,
        CancellationToken cancellationToken);

        Task<IdentityCertificatesDto> GetCertificatesAsync(string accessToken, string tokenType);

        Task<IdentitySessionDto> GetSessionAsync(string accessToken, string tokenType);
    }
}
