using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.PipeMessaging
{
    public abstract class PipeObjectMessagingFactory
    {
        public static PipeObjectMessagingFactory Instance { get; set; }

        public abstract PipeObjectMessaging CreatePipeMessaging(string pipeName);
    }
}
