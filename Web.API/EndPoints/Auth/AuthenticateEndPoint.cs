using Application.Responses;
using Application.UseCases.Portal.Auth;
using Web.API.Requests;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.EndPoints.Auth
{
    public class AuthenticateEndPoint : Endpoint<AuthenticateRequest, AccessTokenResponse>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.Auth));
            Post("api/auth/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AuthenticateRequest req, CancellationToken ct)
        {
            var response =
                await Resolve<IMediator>().Send(new AuthenticateCommand(req.Email, req.Password), ct);
            await SendOkAsync(response, ct);
        }
    }
}
