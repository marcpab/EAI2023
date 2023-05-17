using EAI.PipeMessaging.SAPNcoService.Messaging;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    internal class RfcCallServiceStub : PipeObject, IRfcCallService
    {
        public async Task ConnectAsync(string connectionString, string userName, string password)
        {
            var connectRequest = new ConnectRequest()
            {
                connectionString = connectionString,
                userName = userName,
                password = userName
            };

            var connectResponse = await SendRequest<ConnectResponse>(connectRequest);

            return;
        }

        public async Task DisconnectAsync()
        {
            var disconnectRequest = new DisconnectRequest()
            {
            };

            var disconnectResponse = await SendRequest<DisconnectResponse>(disconnectRequest);

            return;
        }

        public async Task RfcPingAsync()
        {
            var rfcPingRequest = new RfcPingRequest()
            {
            };

            var rfcPingResponse = await SendRequest<RfcPingResponse>(rfcPingRequest);

            return;
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
