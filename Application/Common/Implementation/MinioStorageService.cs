using Application.Common.Interfaces;
using Shared.Settings;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Implementation
{
    public sealed class MinioStorageService(FileServerSettings settings) : IStorageService
    {
        private readonly IMinioClient _minioClient = new MinioClient()
            .WithEndpoint(settings.Endpoint)
            .WithSSL()
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .Build();

        public async Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(settings.Bucket), cancellationToken);

            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(settings.Bucket), cancellationToken);
            }

            await using var stream = file.OpenReadStream();

            var objectName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var resp = await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(settings.Bucket)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType), cancellationToken);

            if (resp.ResponseStatusCode != HttpStatusCode.OK)
                throw new Exception("File could not be saved.");

            return $"https://{settings.Endpoint}/{settings.Bucket}/{objectName}";
        }
    }
}
