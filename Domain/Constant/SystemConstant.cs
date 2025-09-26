using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public class SystemConstant : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }


        public static SystemConstant Create(Guid id, string name, string code, string type, string note, bool isDeleted)
        {
            return new SystemConstant
            {
                Id = id,
                Name = name,
                Code = code,
                Type = type,
                Note = note,
                IsDeleted = isDeleted
            };
        }

        public void Update(Guid id , string? name = null, string? code = null, string? type = null, string? note = null, bool? isAvtive = false)
        {
            Id = id;
            Name = name ?? Name;
            Code = code ?? Code;
            Type = type ?? Type;
            Note = note ?? Note;
            IsDeleted = isAvtive ?? IsDeleted;
        }
    }

}
//GetByType(string type)
// create(SystemConstantmodel model)


