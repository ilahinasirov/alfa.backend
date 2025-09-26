using Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class IncorrectUsernameOrPasswordException()
     : BaseException(SharedResources.ResourceManager.GetString("username_password_incorrect"));
}
