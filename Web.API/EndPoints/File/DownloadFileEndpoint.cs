using Application.UseCases.Constant;
using FastEndpoints;
using MediatR;

namespace Web.API.EndPoints.File
{
    public sealed class DownloadFileEndpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Options(x => x.WithTags(EndpointTags.File));
            Get("/api/file/{objectName}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var objectName = Route<string>("objectName");

            var (fileStream, contentType, fileName) = await Resolve<IMediator>()
                .Send(new DownloadFileCommand(objectName!), ct);
            await SendStreamAsync(
                stream: fileStream,
                fileName: fileName,
                contentType: contentType,
                cancellation: ct
            );
        }

    }
}
