using Application.Common.Interfaces;
using Domain.Constant;
using Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Constant
{
    public  record CreateSystemConstantCommand(Guid id, string Name, string Type, string Code, string Note, bool IsDeleted):IRequest;


    public class CreateSystemConstantCommandHandler(IMessageQueueService producer, IAppDbContext appDbContext) : IRequestHandler<CreateSystemConstantCommand>
    {
        public async Task Handle(CreateSystemConstantCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.SystemConstants.FirstOrDefaultAsync(s=>s.Name==request.Name, cancellationToken:cancellationToken);

            if (entity != null)
            {
                throw new DuplicateDataException();
            }
            entity = SystemConstant.Create(request.id, request.Name,request.Code,request.Type, request.Note, request.IsDeleted);

            await appDbContext.SystemConstants.AddAsync(entity, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }



    }
}
