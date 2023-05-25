using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EAI.General
{
    public class RequestManager<messageT>
        where messageT : IRequestId
    {
        private Dictionary<Guid, TaskCompletionSource<messageT>> _requestMap = new Dictionary<Guid, TaskCompletionSource<messageT>>();

        public TaskCompletionSource<messageT> RegisterRequest(messageT requestMessage)
        {
            requestMessage.RequestId = Guid.NewGuid();
            var requestSync = new TaskCompletionSource<messageT>();

            lock (_requestMap)
                _requestMap.Add(requestMessage.RequestId, requestSync);

            return requestSync;
        }

        public void RequestCompleted(messageT responseMessage)
        {
            lock (_requestMap)
            {
                var requestSync = _requestMap[responseMessage.RequestId];
                _requestMap.Remove(responseMessage.RequestId);

                requestSync.SetResult(responseMessage);
            }
        }

        public void Exception(messageT responseMessage, Exception exception)
        {
            lock (_requestMap)
            {
                var requestSync = _requestMap[responseMessage.RequestId];
                _requestMap.Remove(responseMessage.RequestId);

                requestSync.SetException(new Exception(exception.Message, exception));
            }
        }
    }
}