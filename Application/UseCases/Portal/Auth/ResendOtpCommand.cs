using Application.Common.Interfaces;
using Application.Configuration;
using Application.Responses.Portal.Auth;
using Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.UseCases.Portal.Auth
{
    public class ResendOtpCommand : IRequest<OtpInfoResponse>
    {
        public string Email { get; set; }
    }
    public class ResendOtpCommandHandler(
    IRedisService redisService,
    IMailService mailService,
    IPasswordHasherService passwordHasherService,
    IOptions<OtpOptions> otpOptionsAccessor)
    : IRequestHandler<ResendOtpCommand, OtpInfoResponse>
    {
        public async Task<OtpInfoResponse> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            int _defaultOtpExpiryMinutes = otpOptionsAccessor.Value.OtpExpiryMinutes;
            int _resendOtpRequiredSeconds = otpOptionsAccessor.Value.ResendOtpRequiredSeconds;
            var key = $"otp:{request.Email.ToLower()}";

            var tempUserElement = await redisService.GetAsync<JsonElement>(key);
            if (tempUserElement.ValueKind == JsonValueKind.Undefined || tempUserElement.ValueKind == JsonValueKind.Null)
                throw new DontExistEmailDataAtRedisException();
            var tempUser = tempUserElement.Deserialize<TempUserDto>();
            if ((DateTime.UtcNow - tempUser.CreatedAt).Seconds < _resendOtpRequiredSeconds)
                throw new RequiredOtpTimeException();
            await redisService.RemoveAsync(key);

            var newOtp = new Random().Next(1000, 9999);
            tempUser.Otp = newOtp;
            tempUser.CreatedAt = DateTime.UtcNow;

            await redisService.SetAsync(key, tempUser);

            string subject = "Yeni OTP kodunuz";
            string body = $"Hörmətli {tempUser.Name} {tempUser.Surname},<br>Yeni OTP kodunuz: <b>{newOtp}</b><br>Bu kod {_defaultOtpExpiryMinutes} dəqiqə ərzində keçərlidir.";
            await mailService.SendMailAsync(request.Email, subject, body);

            return new OtpInfoResponse
            {
                OtpCreatedDate = tempUser.CreatedAt,
                OtpExpiredDate = tempUser.CreatedAt.AddMinutes(_defaultOtpExpiryMinutes),
                OtpExpiryMinutes = _defaultOtpExpiryMinutes,
            };
        }
    }
}
