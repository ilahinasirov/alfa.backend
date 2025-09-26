using Application.Common.Interfaces;
using Application.Responses.Portal.Auth;
using Shared.Exceptions;
using MediatR;
using System.Security.Claims;
using Shared.Models.Integration;
using Domain.Identity;
using Application.Common.Implementation;
using Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Responses;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Portal.Auth
{
    public record AuthenticateCommand(string Email, string Password) : IRequest<AccessTokenResponse>;

    public sealed class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AccessTokenResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IAppDbContext _appDbContext;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly AuthSettings _authSettings;

        public AuthenticateCommandHandler(
            ITokenService tokenService,
            IAppDbContext appDbContext,
            IPasswordHasherService passwordHasherService,
            AuthSettings authSettings)
        {
            _tokenService = tokenService;
            _appDbContext = appDbContext;
            _passwordHasherService = passwordHasherService;
            _authSettings = authSettings;
        }

        public async Task<AccessTokenResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.Users
                .SingleOrDefaultAsync(iu => iu.Email == request.Email, cancellationToken)
                ?? throw new IncorrectUsernameOrPasswordException();



            if (!_passwordHasherService.Validate(request.Password, entity.Password))
                throw new IncorrectUsernameOrPasswordException();

            var refreshToken = _tokenService.GenerateRefreshToken();
            entity.RefreshToken = refreshToken;
            entity.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, entity.Id.ToString()),
                new(JwtRegisteredClaimNames.Aud, _authSettings.Audience),
                new(ClaimTypes.Email, entity.Email),
                new(ClaimTypes.Name, entity.Name),
                new(ClaimTypes.Surname, entity.Surname),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            return new AccessTokenResponse
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken,
                IsPasswordChangeRequired = entity?.IsPasswordChangeRequired ?? false,
                IsInitial = entity?.IsInitial ?? true,
            };
        }

    }
}