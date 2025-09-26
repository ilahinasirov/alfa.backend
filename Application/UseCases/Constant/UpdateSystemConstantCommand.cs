using Domain.Constant;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Application.UseCases.Constant
{
    public record UpdateSystemConstantCommand(Guid Id, string Name, string Type, string Code, string Note, bool IsDeleted) : IRequest;


    public class UpdateSystemConstantCommandHandler(IMessageQueueService producer, IAppDbContext appDbContext) : IRequestHandler<UpdateSystemConstantCommand>
    {
        public async Task Handle(UpdateSystemConstantCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.SystemConstants.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken)
             ?? throw new NotFoundEntityException();

            entity.Update(request.Id, request.Name,request.Code,request.Type,request.Note,request.IsDeleted);
            await appDbContext.SaveChangesAsync(cancellationToken);
            producer.SendMessage(entity, "UpdateSystemConstant");

        }
    }
}
