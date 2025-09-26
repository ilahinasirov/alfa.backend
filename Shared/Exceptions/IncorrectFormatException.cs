using Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class IncorrectFormatException(string message = "") : BaseException($"{SharedResources.ResourceManager.GetString("incorrect_format")} {message}");
}
