using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Settings
{
    public sealed class FileServerSettings
    {
        public const string SectionName = nameof(FileServerSettings);

        public string Bucket { get; set; }
        public string Endpoint { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
    }
}
