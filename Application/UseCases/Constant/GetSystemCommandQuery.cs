using Application.Common.Interfaces;
using Application.Responses.Portal.Constant;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Constant
{
    public record GetSystemConstantQuery(string Type) : IRequest<List<SystemConstantResponse>>;

    public sealed class GetSystemConstantQueryHandler(IAppDbContext appDbContext) : IRequestHandler<GetSystemConstantQuery, List<SystemConstantResponse>>
    {
        public async Task<List<SystemConstantResponse>> Handle(GetSystemConstantQuery request, CancellationToken cancellationToken)
        {
            var entities = await appDbContext.SystemConstants.Where(p => p.Type == request.Type).ToListAsync(cancellationToken)
                ?? throw new NotFoundEntityException();

            return entities.Select(entity => new SystemConstantResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Type = entity.Type,
                Note = entity.Note,
                IsDeleted = entity.IsDeleted,
            }).ToList();
        }
    }
}
