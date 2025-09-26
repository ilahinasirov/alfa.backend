using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Integration
{
    public record IdentitySessionDto
    {
        public string LoginType { get; set; }

        public string SessionId { get; set; }
    }
}
