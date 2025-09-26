using Application.Responses.Portal.Constant;
using Application.UseCases.Constant;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.Constants
{
    public class GetSystemConstantEndPoint : EndpointWithoutRequest<List<SystemConstantResponse>>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.SystemConstant));
            Get("api/system-constant/{type}");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var type = Route<string>("type");
            var response = await Resolve<IMediator>().Send(new GetSystemConstantQuery(type), ct);
            await SendOkAsync(response, ct);
        }
    }
}
