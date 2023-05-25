using EAI.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EAI.PipeMessaging
{
    public class PipeMessage : IRequestId
    {
        public PipeActionEnum _action;
        public Guid _instanceId;
        public Guid _requestId;

        public string _payload;
        public string _payloadType;

        [JsonIgnore]
        public Guid RequestId { get => _requestId; set => _requestId = value; }
    }
}
