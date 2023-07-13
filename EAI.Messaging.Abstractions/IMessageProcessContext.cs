using EAI.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Messaging.Abstractions
{
    public interface IMessageProcessContext
    {
        ProcessContext ProcessContext { get; set; }
    }
}
