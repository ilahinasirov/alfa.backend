using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPasswordHasherService
    {
        string Hash(string password);

        bool Validate(string password, string hashPassword);
    }
}
