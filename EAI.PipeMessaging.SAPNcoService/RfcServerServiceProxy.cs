using EAI.PipeMessaging;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public class RfcServerServiceProxy : PipeObject, IRfcServerCallbackAsync
    {
        private IRfcServerService _rfcServerService;

        public RfcServerServiceProxy()
        {
            AddMethod<StartRequest>(r => _rfcServerService.StartAsync(r.connectionString, r.userName, r.password));
            AddMethod<StopRequest>(r => _rfcServerService.StopAsync());
        }

        protected override void SetupRemoteInstance()
        {
            _rfcServerService = (IRfcServerService)PipeMessaging.InstanceFactory.CreateInstance("EAI.NetFramework.SAPNcoService.RfcServerService", "EAI.NetFramework.SAPNcoService");

            _rfcServerService.SetCallback(this);
        }

        public Task ApplicationErrorAsync(Exception error)
        {
            return SendRequest<ApplicationErrorResponse>(new ApplicationErrorRequest
            {
                _error = error
            });
        }

        public Task InvokeFunctionAsync(string functionName, JObject functionData)
        {
            return SendRequest<InvokeFunctionResponse>(new InvokeFunctionRequest
            {
                _functionName = functionName,
                _functionData = functionData
            });
        }

        public Task ServerErrorAsync(Exception error)
        {
            return SendRequest<ServerErrorResponse>(new ServerErrorRequest
            {
                _error = error
            });
        }

        public Task StateChangedAsync(RfcServerStateEnum oldState, RfcServerStateEnum newState)
        {
            return SendRequest<StateChangedResponse>(new StateChangedRequest
            {
                _oldState = oldState,
                _newState = newState
            });
        }
    }

}
