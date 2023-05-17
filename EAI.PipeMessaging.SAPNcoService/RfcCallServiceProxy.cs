using EAI.PipeMessaging;
using EAI.PipeMessaging.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace EAI.Framework.SAPNco
{
    public class RfcCallServiceProxy : PipeObject
    {
        private IRfcCallService _rfcCallService;

        public RfcCallServiceProxy()
        {
            AddMethod<ConnectRequest, ConnectResponse>(r => { _rfcCallService.ConnectAsync(r.connectionString, r.userName, r.password); return null; });
            AddMethod<DisconnectRequest, DisconnectResponse>(r => { _rfcCallService.DisconnectAsync(); return null; });
            AddMethod<RfcPingRequest, RfcPingResponse>(r => { _rfcCallService.RfcPingAsync(); return null; });
            AddMethod<RunJRfcRequest, RunJRfcResponse>(async r => new RunJRfcResponse { _ret = await _rfcCallService.RunJRfcRequestAsync(r.jRfcRequestMessage) });
        }

        protected override void SetupRemoteInstance()
        {
            _rfcCallService = (IRfcCallService)PipeMessaging.InstanceFactory.CreateInstance("EAI.NetFramework.SAPNcoService.RfcCallService", "EAI.NetFramework.SAPNcoService");
        }
    }
}
