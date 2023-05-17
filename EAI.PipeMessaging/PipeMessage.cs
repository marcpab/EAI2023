using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.PipeMessaging
{
    public class PipeMessage
    {
        public PipeActionEnum _action;
        public Guid _instanceId;
        public Guid _requestId;

        public string _payload;
        public string _payloadType;
    }
}
