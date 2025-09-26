using Application.Responses.Portal.Users;
using Application.UseCases.Portal.Users;
using Shared.API;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Users
{
    public class GetUserEndPoint : EndpointWithoutRequest<UserResponse>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.User));
            Get("api/user/");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await Resolve<IMediator>().Send(new GetUserQuery(User.GetId()), ct);
            await SendOkAsync(response, ct);
        }


    }
}
