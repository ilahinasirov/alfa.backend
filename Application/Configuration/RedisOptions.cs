using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class RedisOptions
    {
        public string Configuration { get; set; } = string.Empty;
        public int DataExpiryMinutes { get; set; }
        
    }
}
