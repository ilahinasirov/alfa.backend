using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    public class AuthSettings
    {
        public const string SectionName = nameof(AuthSettings);

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiredInMinutes { get; set; }
    }
}
