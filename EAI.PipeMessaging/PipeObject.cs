using EAI.PipeMessaging.Ping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAI.PipeMessaging
{
    public abstract class PipeObject : IDisposable
    {
        private Guid _instanceId;
        private PipeMessaging _pipeMessaging;
        private bool _removeInstanceOnDispose;

        private Dictionary<string, Func<string, Task<string>>> _methodMap = new Dictionary<string, Func<string, Task<string>>>();
        private bool _disposedValue;

        public Guid InstanceId { get { return _instanceId; } }

        protected PipeMessaging PipeMessaging { get => _pipeMessaging; }

        public PipeObject() 
        { 
            _instanceId = Guid.NewGuid();
        }

        protected async Task CreateRemoteInstance<T>(string pipeName = null)
        {
            _removeInstanceOnDispose = true;

            var requestMessage = new PipeMessage()
            {
                _action = PipeActionEnum.createInstanceRequest,
                _instanceId = _instanceId,
                _payloadType = null,
                _payload = JsonConvert.SerializeObject(
                    new CreateInstanceMessage()
                                {
                                    _assemblyName = typeof(T).Assembly.FullName,
                                    _typeName = typeof(T).FullName
                                }
                            )
            };

            _pipeMessaging = PipeMessaging.GetInstance(pipeName);

            _pipeMessaging.InstanceManager.RegisterInstance(this);

            var requestSync = _pipeMessaging.RequestManager.RegisterRequest(requestMessage);

            _pipeMessaging.SendMessage(requestMessage);

            var responseMessage = await requestSync.Task;

            if (responseMessage._action == PipeActionEnum.exception)
                throw new Exception("Remote execution failed!");

#warning exception;
        }

        internal void SetupRemoteInstance(Guid instanceId, PipeMessaging pipeMessaging)
        {
            _instanceId = instanceId;
            _pipeMessaging = pipeMessaging;

            SetupRemoteInstance(instanceId, _pipeMessaging);
        }

        protected virtual void SetupRemoteInstance()
        {
        }

        protected async Task<responseT> SendRequest<responseT>(object request)
        {
            var requestMessage = new PipeMessage()
            {
                _action = PipeActionEnum.request,
                _instanceId = _instanceId,
                _payloadType = request.GetType().Name,
                _payload = JsonConvert.SerializeObject(request)
            };

            var requestSync = _pipeMessaging.RequestManager.RegisterRequest(requestMessage);

            _pipeMessaging.SendMessage(requestMessage);

            var responseMessage = await requestSync.Task;

            if (responseMessage._action == PipeActionEnum.exception)
                throw new Exception("Remote execution failed!");

#warning exception;

            return JsonConvert.DeserializeObject<responseT>(responseMessage._payload);
        }

        internal void ResponseMessageReceivedAsync(PipeMessage responseMessage)
        {
            _pipeMessaging.RequestManager.RequestCompleted(responseMessage);
        }

        protected void AddMethod<requestT, responseT>(Func<requestT, Task<responseT>> call)
        {
            _methodMap.Add(typeof(requestT).Name, (s) => Call(s, call));
        }

        protected async Task<string> Call<requestT, responseT>(string requestPayload, Func<requestT, Task<responseT>> invoker)
        {
            var request = JsonConvert.DeserializeObject<requestT>(requestPayload);

            var response = await invoker(request);

            return JsonConvert.SerializeObject(response);
        }

        protected internal virtual Task<string> RequestMessageReceivedAsync(string requestPayloadType, string requestPayload)
        {
            return _methodMap[requestPayloadType](requestPayload);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_removeInstanceOnDispose)
                    {
                        _removeInstanceOnDispose = false;

                        var requestMessage = new PipeMessage()
                        {
                            _action = PipeActionEnum.removeInstance,
                            _instanceId = _instanceId
                        };

                        _pipeMessaging.SendMessage(requestMessage);

                        _pipeMessaging.InstanceManager.RemoveInstance(InstanceId);
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PipeObject()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}