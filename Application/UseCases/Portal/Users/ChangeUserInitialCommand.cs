using Application.Common.Interfaces;
using Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Portal.Users
{
    public record ChangeUserInitialCommand(Guid Id) : IRequest;

    public sealed class ChangeUserInitialCommandHandler(IAppDbContext appDbContext, IStorageService storageService) : IRequestHandler<ChangeUserInitialCommand>
    {
        public async Task Handle(ChangeUserInitialCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Users.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                ?? throw new NotFoundEntityException();

            entity.ChangeInitial();
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
