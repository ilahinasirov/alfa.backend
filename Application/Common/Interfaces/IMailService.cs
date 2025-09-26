﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body);
    }

}
