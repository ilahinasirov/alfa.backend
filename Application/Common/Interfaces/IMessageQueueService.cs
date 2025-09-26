using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMessageQueueService
    {
        void SendMessage<T>(T message, string queueName);
    }
}
