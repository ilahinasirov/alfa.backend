using Application.Responses.Portal.Auth;
using Application.UseCases.Portal.Auth;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Auth
{
    public class ResendOtpEndpoint : Endpoint<ResendOtpCommand, OtpInfoResponse>
    {
        public override void Configure()
        {
            Post("api/auth/resend-otp");
            AllowAnonymous();
            Options(x => x.WithTags(EndpointTags.Auth));
        }

        public override async Task HandleAsync(ResendOtpCommand req, CancellationToken ct)
        {
            var response = await Resolve<IMediator>().Send(req, ct);
            await SendOkAsync(response, ct);
        }
    }

}
