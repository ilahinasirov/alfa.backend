using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Portal.Constant
{
    public sealed class SystemConstantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
