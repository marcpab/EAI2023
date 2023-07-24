using EAI.PipeMessaging;
using EAI.PipeMessaging.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public class RfcCallServiceProxy : PipeObject
    {
        private IRfcCallService _rfcCallService;

        public RfcCallServiceProxy()
        {
            AddMethod<ConnectRequest>(r => _rfcCallService.ConnectAsync(r.connectionString, r.userName, r.password));
            AddMethod<DisconnectRequest>(r => _rfcCallService.DisconnectAsync());
            AddMethod<RfcPingRequest>(r => _rfcCallService.RfcPingAsync());
            AddMethod<RunJRfcRequest, RunJRfcResponse>(async r => new RunJRfcResponse { _ret = await _rfcCallService.RunJRfcRequestAsync(r.jRfcRequestMessage, r.autoCommit) });
            AddMethod<GetJRfcSchemaRequest, GetJRfcSchemaResponse>(async r => new GetJRfcSchemaResponse { _ret = await _rfcCallService.GetJRfcSchemaAsync(r.functionName) });
            AddMethod<GetRfcFunctionMetadataRequest, GetRfcFunctionMetadataResponse>(async r => new GetRfcFunctionMetadataResponse { _ret = await _rfcCallService.GetRfcFunctionMetadataAsync(r.functionName) });
        }

        protected override void SetupRemoteInstance()
        {
            _rfcCallService = (IRfcCallService)PipeMessaging.InstanceFactory.CreateInstance("EAI.NetFramework.SAPNcoService.RfcCallService", "EAI.NetFramework.SAPNcoService");
        }
    }
}
