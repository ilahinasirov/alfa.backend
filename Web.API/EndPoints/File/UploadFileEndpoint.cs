using Application.Responses.Portal.Constant;
using Application.UseCases.Constant;
using Web.API.Requests.Constant;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.File
{
    public sealed class UploadFileEndpoint : Endpoint<UpdateFileRequest, FileResponse>
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.File));
            Post("/api/file");
            AllowFileUploads();
        }

        public override async Task HandleAsync(UpdateFileRequest req, CancellationToken ct)
        {
            var response = await Resolve<IMediator>().Send(new UploadFileCommand(req.File), ct);
            await SendOkAsync(response, ct);
        }
    }
}
