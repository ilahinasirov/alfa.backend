using Application.Common.Interfaces;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UseCases.Portal.Auth
{
    public record ChangePasswordCommand(Guid Id, string Password, string NewPassword, string ConfirmPassword) : IRequest;

    public sealed class ChangePasswordCommandHandler(
    IMessageQueueService producer, IAppDbContext appDbContext, IPasswordHasherService passwordHasherService) : IRequestHandler<ChangePasswordCommand>
    {
        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new NotFoundEntityException();
          



            if (!passwordHasherService.Validate(request.Password, entity.Password))
            {
                throw new IncorrectCurrentPasswordException();
            }
            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new DontMatchPasswordException();
            }
            if (!IsValidPassword(request.NewPassword))
            {
                throw new InvalidPasswordFormatException();
            }
            var newPasswordHash = passwordHasherService.Hash(request.NewPassword);

            entity.ChangePassword(newPasswordHash);
            var result = await appDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0) producer.SendMessage(entity, "ChangePassword");
        }
        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$");
             bool isMatch= passwordRegex.IsMatch(password);
            return isMatch;
        }
    }
}
