using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Pin { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset? PasswordExpiryDate { get; private set; }
        public string? Patronymic { get; set; }
        public string Status { get; set; }
        public string? Gender { get; set; }
        public string? PhotoPath { get; set; }
        public string? MartialStatus { get; set; }
        public string? CurrentAddress { get; set; }
        public string? RegisterAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
    }
}
