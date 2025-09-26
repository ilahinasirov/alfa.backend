using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken);
    }
}
