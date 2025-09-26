using Application.Common.Implementation;
using Application.Common.Interfaces;
using Application.Configuration;
using Domain.Identity;
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
    public class TempUserDto
    {
        public string Pin { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Otp { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CompleteRegisterRequestCommand : IRequest
    {
        public string Email { get; set; }
        public int Otp { get; set; }
    }

    public class CompleteRegisterRequestCommandHandler(
    IRedisService redisService,
    IAppDbContext db,
    IMessageQueueService messageQueue,
    IOptions<OtpOptions> otpOptionsAccessor,
        IMailService _mailService)
    : IRequestHandler<CompleteRegisterRequestCommand>
    {
        private readonly int OtpValidMinutes = otpOptionsAccessor.Value.OtpExpiryMinutes;
        public async Task Handle(CompleteRegisterRequestCommand request, CancellationToken cancellationToken)
        {
            var key = $"otp:{request.Email.ToLower()}";

            var tempUserElement = await redisService.GetAsync<JsonElement>(key);


            if (tempUserElement.ValueKind == JsonValueKind.Undefined || tempUserElement.ValueKind == JsonValueKind.Null)
                throw new OtpExpiredOrVerificationException();

            var tempUser = tempUserElement.Deserialize<TempUserDto>();

            if (tempUser == null)
                throw new OtpExpiredOrVerificationException();

            var elapsedSeconds = (DateTime.UtcNow - tempUser.CreatedAt).TotalSeconds;
            if (elapsedSeconds > OtpValidMinutes * 60)
            {
                await redisService.RemoveAsync(key);
                throw new OtpExpiredException();
            }

            if (tempUser.Otp != request.Otp)
                throw new OtpVerificationException();

            var entity = User.Create(
                tempUser.Pin,
                tempUser.Name,
                tempUser.Surname,
                string.Empty,
                tempUser.Email,
                tempUser.Password,
                "Active",
                string.Empty, string.Empty, string.Empty, string.Empty, null,
                 string.Empty, false, null, true);

            await db.Users.AddAsync(entity, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            string subject = "Qeydiyyatın uğurla tamamlanması";
            string body = $"Hörmətli {tempUser.Name} {tempUser.Surname},<br>qeydiyyatınız uğurla tamamlandı<b></b><br>Artıq portala giriş edə bilərsiniz.";
            await _mailService.SendMailAsync(request.Email, subject, body);
            await redisService.RemoveAsync(key);

            messageQueue.SendMessage(entity, "RegisterUser");
        }
    }
}
