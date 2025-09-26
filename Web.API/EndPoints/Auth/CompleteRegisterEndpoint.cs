using Application.UseCases.Portal.Auth;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Auth
{
    public class CompleteRegisterEndpoint : Endpoint<CompleteRegisterRequestCommand>
    {
        public override void Configure()
        {
            Post("api/auth/complete-register");
            AllowAnonymous();
            Options(x => x.WithTags(EndpointTags.Auth));
        }

        public override async Task HandleAsync(CompleteRegisterRequestCommand req, CancellationToken ct)
        {
            await Resolve<IMediator>().Send(req, ct);
            await SendNoContentAsync(ct);
        }
    }

}
