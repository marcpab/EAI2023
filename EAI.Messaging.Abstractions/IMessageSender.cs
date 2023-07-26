using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Messaging.Abstractions
{
    public interface IMessageSender
    {
        Task SendMessageAsync(object message);
    }
}
