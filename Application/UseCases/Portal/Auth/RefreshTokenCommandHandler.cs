using Application.Common.Interfaces;
using Application.Responses;
using Shared.Settings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Portal.Auth
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AccessTokenResponse>;
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessTokenResponse>
    {
        private readonly IAppDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly AuthSettings _authSettings;
        public RefreshTokenCommandHandler(IAppDbContext dbContext, ITokenService tokenService, AuthSettings authSettings)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _authSettings = authSettings;
        }
        public async Task<AccessTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var email = principal?.FindFirst(ClaimTypes.Email)?.Value
                 ?? throw new UnauthorizedAccessException("Invalid access token");

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken)
                ?? throw new UnauthorizedAccessException("User Not found");
            if (user.RefreshToken != request.RefreshToken && user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new AccessTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

        }
    }
}
