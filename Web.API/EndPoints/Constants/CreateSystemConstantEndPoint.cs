using Application.UseCases.Constant;
using Web.API.Requests.Constant;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Constants
{
    public class CreateSystemConstantEndPoint : Endpoint<CreateSystemConstantRequest>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.SystemConstant));
            Post("api/system-constant");
        }
        public override async Task HandleAsync(CreateSystemConstantRequest req, CancellationToken ct)
        {
            await Resolve<IMediator>().Send(new CreateSystemConstantCommand(req.Id,req.Name,req.Type, req.Code,req.Note,req.IsDeleted), ct);
            await SendNoContentAsync(ct);
        }
    }
}
