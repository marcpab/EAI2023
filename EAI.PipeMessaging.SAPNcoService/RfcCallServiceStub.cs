using EAI.PipeMessaging.Ping;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public class RfcCallServiceStub : PipeObject, IRfcCallService
    {
        public static async Task<IRfcCallService> CreateObjectAsync(string pipeName = null)
        {
            var stub = new RfcCallServiceStub();

            await stub.CreateRemoteInstance<RfcCallServiceProxy>(pipeName);

            return stub;
        }

        public Task ConnectAsync(string connectionString, string userName, string password)
        {
            var connectRequest = new ConnectRequest()
            {
                connectionString = connectionString,
                userName = userName,
                password = userName
            };

            return SendRequest<ConnectResponse>(connectRequest);
        }

        public Task DisconnectAsync()
        {
            var disconnectRequest = new DisconnectRequest()
            {
            };

            return SendRequest<DisconnectResponse>(disconnectRequest);
        }

        public Task RfcPingAsync()
        {
            var rfcPingRequest = new RfcPingRequest()
            {
            };

            return SendRequest<RfcPingResponse>(rfcPingRequest);
        }

        public async Task<string> RunJRfcRequestAsync(string jRfcRequestMessage)
        {
            var runJRfcRequest = new RunJRfcRequest()
            {
                jRfcRequestMessage = jRfcRequestMessage
            };

            var runJRfcResponse = await SendRequest<RunJRfcResponse>(runJRfcRequest);

            return runJRfcResponse._ret;
        }
    }
}
