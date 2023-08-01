using EAI.PipeMessaging;
using EAI.PipeMessaging.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public class RfcGatewayServiceProxy : PipeObject, IRfcServerCallbackAsync
    {
        private IRfcGatewayService _rfcGatewayService;

        public RfcGatewayServiceProxy()
        {
            AddMethod<ConnectRequest>(r => _rfcGatewayService.ConnectAsync(r.connectionString, r.userName, r.password));
            AddMethod<DisconnectRequest>(r => _rfcGatewayService.DisconnectAsync());
            AddMethod<RfcPingRequest>(r => _rfcGatewayService.RfcPingAsync());
            AddMethod<RunJRfcRequest, RunJRfcResponse>(async r => new RunJRfcResponse { _ret = await _rfcGatewayService.RunJRfcRequestAsync(r.jRfcRequestMessage, r.autoCommit) });
            AddMethod<GetJRfcSchemaRequest, GetJRfcSchemaResponse>(async r => new GetJRfcSchemaResponse { _ret = await _rfcGatewayService.GetJRfcSchemaAsync(r.functionName) });
            AddMethod<GetRfcFunctionMetadataRequest, GetRfcFunctionMetadataResponse>(async r => new GetRfcFunctionMetadataResponse { _ret = await _rfcGatewayService.GetRfcFunctionMetadataAsync(r.functionName) });
            AddMethod<StartServerRequest>(r => _rfcGatewayService.StartServerAsync());
            AddMethod<StopServerRequest>(r => _rfcGatewayService.StopServerAsync());
        }

        protected override void SetupRemoteInstance()
        {
            _rfcGatewayService = (IRfcGatewayService)PipeMessaging.InstanceFactory.CreateInstance("EAI.NetFramework.SAPNcoService.RfcCallService", "EAI.NetFramework.SAPNcoService");

            _rfcGatewayService.SetServerCallback(this);
        }

        public Task ApplicationErrorAsync(ExceptionData error)
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

        public Task ServerErrorAsync(ExceptionData error)
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
