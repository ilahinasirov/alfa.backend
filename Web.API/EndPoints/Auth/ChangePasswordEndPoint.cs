using Application.UseCases.Portal.Auth;
using Shared.API;
using Web.API.Requests.Users;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Auth
{
    public class ChangePasswordEndPoint : Endpoint<ChangePasswordRequest>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.Auth));
            Put("api/change-password/");
        }
        public override async Task HandleAsync(ChangePasswordRequest req, CancellationToken ct)
        {
            await Resolve<IMediator>().Send(new ChangePasswordCommand(User.GetId(),req.OldPassword,req.NewPassword,req.ConfirmPassword), ct);
            await SendNoContentAsync(ct);
        }

    }
}
