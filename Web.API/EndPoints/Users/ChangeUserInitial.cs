using Application.UseCases.Portal.Users;
using Shared.API;
using Web.API.Requests.Users;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Users
{
    public class ChangeUserInitial : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.User));
            Put("api/user/change-initial");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await Resolve<IMediator>().Send(new ChangeUserInitialCommand(User.GetId()), ct);
            await SendNoContentAsync(ct);
        }
    }
}
