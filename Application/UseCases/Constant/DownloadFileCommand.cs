using Application.Common.Interfaces;
using MediatR;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Constant
{
    public sealed record DownloadFileCommand(string ObjectName) : IRequest<(Stream FileStream, string ContentType, string FileName)>;

    public sealed class DownloadFileRequestHandler(IStorageService storageService)
        : IRequestHandler<DownloadFileCommand, (Stream, string, string)>
    {
        public async Task<(Stream, string, string)> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ObjectName))
                throw new InvalidFileStrorage();

            var memoryStream = await storageService.DownloadAsync(request.ObjectName);

            var contentType = "application/octet-stream";
            var fileName = Path.GetFileName(request.ObjectName);

            return (memoryStream, contentType, fileName);
        }
    }
}
