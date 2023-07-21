using System;

namespace EAI.PipeMessaging.SAPNcoService.Messaging
{
    internal class ServerErrorRequest
    {
        public Exception _error { get; set; }
    }
}