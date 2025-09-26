using Application.Common.Interfaces;
using Application.Responses.Portal.Constant;
using Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Constant
{
    public sealed record UploadFileCommand(IFormFile File) : IRequest<FileResponse>;

    public sealed class UploadFileRequestHandler(IStorageService storageService) : IRequestHandler<UploadFileCommand, FileResponse>
    {
        public async Task<FileResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
             const long MaxFileSize = 4 * 1024 * 1024;
            if (request.File.Length > MaxFileSize) {
                throw new InvalidFileStrorage();
            }

            var path = await storageService.SaveAsync(request.File, cancellationToken);
            return new FileResponse(request.File.FileName, path);
        }
    }
}
