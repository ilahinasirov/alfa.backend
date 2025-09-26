using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Implementation
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Validate(string password, string hashPassword) => BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}
