using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ErrorResponse
    {
        public string TraceId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public int ErrorCode { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
