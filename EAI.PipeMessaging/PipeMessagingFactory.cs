using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.PipeMessaging
{
    public abstract class PipeMessagingFactory
    {
        public static PipeMessagingFactory Instance { get; set; }

        public abstract PipeMessaging CreatePipeMessaging(string pipeName);
    }
}
