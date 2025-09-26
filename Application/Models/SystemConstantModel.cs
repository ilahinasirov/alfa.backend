using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class SystemConstantModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public bool IsNew { get; set; }
    }
}
