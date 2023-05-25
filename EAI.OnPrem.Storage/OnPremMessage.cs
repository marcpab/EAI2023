using EAI.General;
using Newtonsoft.Json;
using System;

namespace EAI.OnPrem.Storage
{
    public class OnPremMessage : IRequestId
    {
        public Guid _requestId;
        public string _responseQueueName;
        public string _payload;

        [JsonIgnore]
        public Guid RequestId { get => _requestId; set => _requestId = value; }
    }
}