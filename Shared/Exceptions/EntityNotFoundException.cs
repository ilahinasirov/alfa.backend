using Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class EntityNotFoundException(string message)
    : NotFoundException($"{SharedResources.ResourceManager.GetString("object_not_found")} {message}");
}
