using Web.API.Requests;
using FastEndpoints;
using MediatR;
using Application.UseCases.Portal.Auth;
using Application.Responses.Portal.Auth;

namespace Web.API.EndPoints.Auth
{
    public class RegisterRequestEndpoint : Endpoint<RegisterRequest,OtpInfoResponse>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.Auth));
            Post("api/auth/register-request");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            var response = await Resolve<IMediator>().Send(new RegisterRequestCommand
            {
                Pin = req.Pin,
                Name = req.Name,
                Surname = req.Surname,
                Email = req.Email,
                Password = req.Password,
            }, ct);

            await SendOkAsync(response, ct);
        }
    }
}
