using Application.Responses;
using Application.UseCases.Portal.Auth;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Auth
{
    public class RefreshTokenEndPoint : Endpoint<RefreshTokenCommand, AccessTokenResponse>
    {
        public override void Configure()
        {
            Post("api/auth/refresh-token");
            AllowAnonymous();
            Options(x => x.WithTags(EndpointTags.Auth));
        }

        public override async Task HandleAsync(RefreshTokenCommand req, CancellationToken ct)
        {
            var response = await Resolve<IMediator>().Send(req, ct);
            await SendOkAsync(response, ct);
        }
    }
}
