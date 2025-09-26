using Shared.Exceptions;
using System.Security.Claims;

namespace Shared.API
{
    public static class ApiExtensions
    {
        public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
                throw new EntityNotFoundException(nameof(ClaimsPrincipal));

            return Guid.Parse(id);
        }
    }
}
