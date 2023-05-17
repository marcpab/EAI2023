using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EAI.PipeMessaging
{
    public class RequestManager
    {
        private Dictionary<Guid, TaskCompletionSource<PipeMessage>> _requestMap = new Dictionary<Guid, TaskCompletionSource<PipeMessage>>();

        public TaskCompletionSource<PipeMessage> RegisterRequest(PipeMessage requestMessage)
        {
            requestMessage._requestId = Guid.NewGuid();
            var requestSync = new TaskCompletionSource<PipeMessage>();

            lock (_requestMap)
                _requestMap.Add(requestMessage._requestId, requestSync);

            return requestSync;
        }

        public void RequestCompleted(PipeMessage responseMessage)
        {
            lock (_requestMap)
            {
                var requestSync = _requestMap[responseMessage._requestId];
                _requestMap.Remove(responseMessage._requestId);

                requestSync.SetResult(responseMessage);
            }
        }
    }
}