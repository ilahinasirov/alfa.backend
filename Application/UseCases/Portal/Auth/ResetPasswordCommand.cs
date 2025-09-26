using Application.Common.Interfaces;
using Application.Configuration;
using Application.Responses.Portal.Auth;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UseCases.Portal.Auth
{
    public record ResetPasswordCommand(string Email) : IRequest;
    
    public class ResetPasswordCommandHandler(
   IMessageQueueService producer,
   IAppDbContext appDbContext,
   IPasswordHasherService passwordHasherService,
   IMailService _mailService)
   : IRequestHandler<ResetPasswordCommand>
    {
        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken) ??
                throw new NotFoundEntityException();

            string newPassword = GenerateSecurePassword();
            var password = passwordHasherService.Hash(newPassword);
            user.ResetPassword(password);
            await appDbContext.SaveChangesAsync(cancellationToken);
            string subject = "Şifrənin bərpası üçün kod";
            string body = $"Hörmətli {user.Name} {user.Surname},<br>Sizin birdəfəlik şifrəniz: <b>{newPassword}</b>";
            await _mailService.SendMailAsync(request.Email, subject, body);
        }
        private string GenerateSecurePassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string symbols = "!@#$%&*_+-=.?";
            const int length = 8;

            var random = new Random();
            var chars = new List<char>
            {
                upper[random.Next(upper.Length)],
                lower[random.Next(lower.Length)],
                digits[random.Next(digits.Length)],
                symbols[random.Next(symbols.Length)]
            };
            string allChars = upper + lower + digits + symbols;
            while (chars.Count < length)
            {
                chars.Add(allChars[random.Next(allChars.Length)]);
            }
            return new string(chars.OrderBy(_ => random.Next()).ToArray());
        }

    }
}
