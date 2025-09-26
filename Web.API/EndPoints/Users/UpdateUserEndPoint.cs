using Application.UseCases.Portal.Users;
using Shared.API;
using Web.API.Requests.Users;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Users
{
    public class UpdateUserEndPoint : Endpoint<UpdateUserRequest>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.User));
            Put("api/user/");
        }
        public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
        {
            await Resolve<IMediator>().Send(new UpdateUserCommand(User.GetId(), req.Name, req.Surname, req.Patronymic, req.Gender, req.MartialStatus, req.CurrentAddress, req.RegisterAddress,req.BirthDate,req.Photo,req.IsPasswordChangeRequired), ct);
            await SendNoContentAsync(ct);
        }

    }
}
