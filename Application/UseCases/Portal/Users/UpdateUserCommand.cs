using Application.Common.Interfaces;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using Domain.Identity;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Portal.Users
{
    public record UpdateUserCommand(Guid Id, string Name, string Surname, string Patronymic, string Gender, string MartialStatus, string CurrentAddress, string RegisterAddress,DateTime BirthDate,string? PhotoFile,bool isPasswordChangeRequired) : IRequest;

    public sealed class UpdateUserCommandHandler(IMessageQueueService producer, IAppDbContext appDbContext, IStorageService storageService) : IRequestHandler<UpdateUserCommand>
    {
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new NotFoundEntityException();
            
            entity.Update(request.Name, request.Surname, request.Patronymic, request.Gender, request.MartialStatus, request.CurrentAddress, request.RegisterAddress,request.BirthDate,request.PhotoFile,request.isPasswordChangeRequired);
            await appDbContext.SaveChangesAsync(cancellationToken);
            var updatedEntity = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            producer.SendMessage(updatedEntity, "UpdateUser");

        }
    }
}
