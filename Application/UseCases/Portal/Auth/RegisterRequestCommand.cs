using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Common.Implementation;
using Application.Common.Interfaces;
using Application.Configuration;
using Application.Responses.Portal.Auth;
using Domain.Identity;
using Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.UseCases.Portal.Auth
{
    public class RegisterRequestCommand : IRequest<OtpInfoResponse>
    {

        public string Pin { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }


    }

    public class RegisterRequestCommandHandler(
    IMessageQueueService producer,
    IAppDbContext appDbContext,
    IPasswordHasherService passwordHasherService,
    IMailService _mailService,
    IRedisService redisService,
    IOptions<RedisOptions> redisOptionsAccessor,
    IOptions<OtpOptions> otpOptionsAccessor)
    : IRequestHandler<RegisterRequestCommand,OtpInfoResponse>
    {
        private readonly int _defaultDataExpiryMinutes = redisOptionsAccessor.Value.DataExpiryMinutes;
        public async Task<OtpInfoResponse> Handle(RegisterRequestCommand request, CancellationToken cancellationToken)
        {
            int _defaultOtpExpiryMinutes = otpOptionsAccessor.Value.OtpExpiryMinutes;
            bool pinExists = await appDbContext.Users.AnyAsync(u => u.Pin == request.Pin, cancellationToken);

            bool emailExists = await appDbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (pinExists && emailExists)
                throw new DuplicateFinAndEmailException();
            if(pinExists)
                throw new DuplicateFinException();
            if (emailExists)
                throw new DuplicateEmailException();


            if (!IsValidPassword(request.Password))
            {
                throw new InvalidPasswordFormatException();
            }
            var otp = new Random().Next(1000, 9999);
            var hashedPassword = passwordHasherService.Hash(request.Password);

            var tempUser = new
            {
                request.Pin,
                request.Name,
                request.Surname,
                request.Email,
                Password = hashedPassword,
                Otp = otp,
                CreatedAt= DateTime.UtcNow
            };

            var key = $"otp:{request.Email.ToLower()}";
            await redisService.SetAsync(key, tempUser, TimeSpan.FromMinutes(_defaultDataExpiryMinutes));

            string subject = "Qeydiyyat üçün OTP kod";
            string body = $"Hörmətli {request.Name} {request.Surname},<br>Doğrulama kodunuz: <b>{otp}</b><br>Bu kod {_defaultOtpExpiryMinutes} dəqiqə ərzində keçərlidir.";
            await _mailService.SendMailAsync(request.Email, subject, body);
            return new OtpInfoResponse
            {
                OtpCreatedDate = tempUser.CreatedAt,
                OtpExpiredDate=tempUser.CreatedAt.AddMinutes(_defaultOtpExpiryMinutes),
                OtpExpiryMinutes = _defaultOtpExpiryMinutes,
            };

        }
        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$");
            return passwordRegex.IsMatch(password);
        }
    }
}
