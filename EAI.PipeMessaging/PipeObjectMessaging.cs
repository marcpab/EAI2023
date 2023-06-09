﻿using EAI.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.PipeMessaging
{
    public class PipeObjectMessaging
    {
        private static Dictionary<string, PipeObjectMessaging> _pipeMessagingMap = new Dictionary<string, PipeObjectMessaging>();

        public static string DefaultPipeName { get; set; } = "EAIPipe";

        public static PipeObjectMessaging GetInstance(string pipeName)
        {
            if (pipeName == null)
                pipeName = DefaultPipeName;

            lock(_pipeMessagingMap)
            {
                PipeObjectMessaging pipeMessaging;
                if (_pipeMessagingMap.TryGetValue(pipeName, out pipeMessaging))
                    return pipeMessaging;

                pipeMessaging = PipeObjectMessagingFactory.Instance.CreatePipeMessaging(pipeName);

                _pipeMessagingMap.Add(pipeName, pipeMessaging);

                return pipeMessaging;
            }
        }
       
        private InstanceManager _instanceManager;
        private RequestManager<PipeMessage> _requestManager;
        private IInstanceFactory _instanceFactory;
        private string _pipeName;

        private PipeBidirectional[] _pipes;
        private int _pipeIndex;

        public InstanceManager InstanceManager { get => _instanceManager; }
        public RequestManager<PipeMessage> RequestManager { get => _requestManager; }
        public IInstanceFactory InstanceFactory { get => _instanceFactory; }

        public string PipeName { get => _pipeName; }
        public PipeBidirectional[] Pipes { get => _pipes; }

        protected void Setup(InstanceManager instanceManager, RequestManager<PipeMessage> requestManager, IInstanceFactory instanceFactory)
        {
            _instanceManager = instanceManager;
            _requestManager = requestManager;
            _instanceFactory = instanceFactory;
        }

        protected void CreatePipes(string pipeName, int pipeCount)
        {
            _pipeName = pipeName;

            _pipes = Enumerable
                        .Repeat<PipeBidirectional>(
                                new PipeBidirectional() { 
                                    PipeName = pipeName, 
                                    PipeCount = pipeCount 
                                }, pipeCount)
                        .ToArray();
        }

        protected Task MessageReceivedAsync(PipeMessage message)
        {
            switch (message._action)
            {
                case PipeActionEnum.createInstance:
                    ProcessCreateInstance(message);
                    return Task.CompletedTask;

                case PipeActionEnum.request:
                    return ProcessRequestAsync(message);

                case PipeActionEnum.removeInstance:
                    ProcessRemoveInstance(message);
                    return Task.CompletedTask;

                case PipeActionEnum.response:
                    ProcessResponse(message);
                    return Task.CompletedTask;

                case PipeActionEnum.exception:
                    ProcessException(message);
                    return Task.CompletedTask;

                case PipeActionEnum.shutdown:
                    Shutdown();
                    return Task.CompletedTask;
                default:
                    return Task.CompletedTask;
            }
        }

        private void ProcessCreateInstance(PipeMessage message)
        {
            try
            {
                var createInstance = JsonConvert.DeserializeObject<CreateInstanceMessage>(message._payload);

                var instance = (PipeObject)_instanceFactory.CreateInstance(createInstance._typeName, createInstance._assemblyName);

                instance.SetupRemoteInstance(message._instanceId, this);

                _instanceManager.RegisterInstance(instance);

                message._action = PipeActionEnum.response;
            }
            catch (Exception ex)
            {
                SetException(message, ex);
            }

            SendMessage(message);
        }

        private async Task ProcessRequestAsync(PipeMessage message)
        {
            var instance = _instanceManager.GetInstance(message._instanceId);

            try
            {
                message._action = PipeActionEnum.response;
                message._payload = await instance.RequestMessageReceivedAsync(message._payloadType, message._payload);
            }
            catch (Exception ex)
            {
                SetException(message, ex);
            }

            SendMessage(message);
        }

        private static void SetException(PipeMessage message, Exception ex)
        {
            var pipeExceptionMessage = PipeExceptionMessage.FromException(ex);

            message._action = PipeActionEnum.exception;
            message._payload = JsonConvert.SerializeObject(pipeExceptionMessage);
        }

        private static Exception GetException(PipeMessage message)
        {
            var pipeExceptionMessage = JsonConvert.DeserializeObject< PipeExceptionMessage>(message._payload);

            var exception = PipeExceptionMessage.ToException(pipeExceptionMessage);

            return exception;
        }

        private void ProcessRemoveInstance(PipeMessage message)
        {
            var instance = _instanceManager.RemoveInstance(message._instanceId);

            (instance as IDisposable)?.Dispose();
        }

        private void ProcessResponse(PipeMessage message)
        {
            var instance = _instanceManager.GetInstance(message._instanceId);

            instance.ResponseMessageReceived(message);
        }

        private void ProcessException(PipeMessage message)
        {
            var instance = _instanceManager.GetInstance(message._instanceId);

            var exception = GetException(message);

            instance.ExceptionMessageReceived(message, exception);
        }

        protected virtual void Shutdown()
        {
        }

        internal void SendMessage(PipeMessage requestMessage)
        {
            _pipes[_pipeIndex].EnqueueMessage(requestMessage);
            
            if(++_pipeIndex == _pipes.Length)
                _pipeIndex = 0;
        }
    }
}
