using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Portal.Users
{
    public sealed class UserResponse
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public string CurrentAddress { get; set; }
        public string RegisterAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Photo { get; set; }
        public string State { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
        public bool? IsInitial { get; set; }


    }
}
