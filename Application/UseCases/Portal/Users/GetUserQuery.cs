using Application.Common.Interfaces;
using Application.Responses.Portal.Users;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Portal.Users
{
    public record GetUserQuery(Guid Id) : IRequest<UserResponse>;

    public sealed class GetUserQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetUserQuery, UserResponse>
    {
        public async Task<UserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new NotFoundEntityException();

            bool userTask = await appDbContext.Users.AnyAsync(p => p.Id == request.Id && !string.IsNullOrWhiteSpace(p.Gender), cancellationToken);
           

            string state = "Default";
            if (userTask) state = "UserCompleted";
            return new UserResponse
            {
                Name = entity.Name,
                SurName = entity.Surname,
                MartialStatus = entity.MartialStatus,
                Gender = entity.Gender,
                CurrentAddress = entity.CurrentAddress,
                RegisterAddress = entity.RegisterAddress,
                BirthDate = entity.BirthDate,
                Patronymic = entity.Patronymic,
                Photo= entity.PhotoPath,
                State = state,
                IsPasswordChangeRequired = entity.IsPasswordChangeRequired,
                IsInitial = entity.IsInitial
            };
        }
    }
}
