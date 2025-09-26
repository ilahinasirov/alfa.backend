using Application.UseCases.Portal.Auth;
using Shared.API;
using Web.API.Requests;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Auth
{
    public class ResetPasswordEndPoint : Endpoint<ResetPasswordRequest>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.Auth));
            Put("api/auth/reset-password");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
        {
                await Resolve<IMediator>().Send(new ResetPasswordCommand(req.Email), ct);
            await SendOkAsync(ct);
        }
    }
}
