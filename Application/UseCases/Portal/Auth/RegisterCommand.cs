using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Common.Implementation;
using Application.Common.Interfaces;
using Application.Responses.Portal.Auth;
using Domain.Identity;
using Shared.Exceptions;
using DocumentFormat.OpenXml.Office2010.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Portal.Auth
{
    public class RegisterCommand : IRequest
    {
        public RegisterCommand() { }
        public RegisterCommand(Guid id, string pin, string name, string surname, string email, string password, string status,bool isPasswordChangeRequired, string? patronymic = null)
        {
            Id = id;
            Pin = pin;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Status = status;
            IsPasswordChangeRequired = isPasswordChangeRequired;
            Patronymic = patronymic != null ? patronymic : string.Empty;
        }

        public Guid Id { get; set; }
        public string Pin { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public bool IsPasswordChangeRequired { get; set; }

    }

    public class RegisterCommandHandler(
    IMessageQueueService producer,
    IAppDbContext appDbContext,
    IPasswordHasherService passwordHasherService)
    : IRequestHandler<RegisterCommand>
    {

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Users.FirstOrDefaultAsync(iu => iu.Email == request.Email || iu.Pin == request.Pin,
            cancellationToken: cancellationToken);

            if (entity != null)
                throw new DuplicateUserNameException();
            if (!IsValidPassword(request.Password))
            {
                throw new InvalidPasswordFormatException();
            }

            var hashedPassword = passwordHasherService.Hash(request.Password);
            entity = User.Create(request.Pin, request.Name, request.Surname, request.Patronymic, request.Email, hashedPassword, request.Status,string.Empty, string.Empty, string.Empty, string.Empty,null,string.Empty,request.IsPasswordChangeRequired, request.Id, false);

            await appDbContext.Users.AddAsync(entity, cancellationToken);
            var result = await appDbContext.SaveChangesAsync(cancellationToken);    
            if (result > 0) producer.SendMessage(entity, "RegisterUser");
        }
        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$");
            return passwordRegex.IsMatch(password);
        }
    }
}
