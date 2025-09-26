using Application.UseCases.Constant;
using Web.API.Requests.Constant;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Constants
{
    public class UpdateSystemConstantEndPoint : Endpoint<UpdateSystemConstantRequest>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.SystemConstant));
            Put("api/system-constant");
        }
        public override async Task HandleAsync(UpdateSystemConstantRequest req, CancellationToken ct)
        {
            await Resolve<IMediator>().Send(new UpdateSystemConstantCommand(req.Id,req.Name, req.Type, req.Code, req.Note, req.IsDeleted), ct);
            await SendNoContentAsync(ct);
        }
    }
}
