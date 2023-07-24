using EAI.PipeMessaging.Ping;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public class RfcServerServiceStub : PipeObject, IRfcServerService
    {
        public static async Task<IRfcServerService> CreateObjectAsync(string pipeName = null)
        {
            var stub = new RfcServerServiceStub();

            await stub.CreateRemoteInstance<RfcServerServiceProxy>(pipeName);

            return stub;
        }

        private IRfcServerCallbackAsync _rfcServerCallback;

        private RfcServerServiceStub()
        {
            AddMethod<ApplicationErrorRequest, ApplicationErrorResponse>(async r =>
            {
                await _rfcServerCallback.ApplicationErrorAsync(r._error);

                return new ApplicationErrorResponse();
            });

            AddMethod<InvokeFunctionRequest, InvokeFunctionResponse>(async r =>
            {
                await _rfcServerCallback.InvokeFunctionAsync(r._functionName, r._functionData);

                return new InvokeFunctionResponse();
            });

            AddMethod<ServerErrorRequest, ServerErrorResponse>(async r =>
            {
                await _rfcServerCallback.ServerErrorAsync(r._error);

                return new ServerErrorResponse();
            });

            AddMethod<StateChangedRequest, StateChangedResponse>(async r =>
            {
                await _rfcServerCallback.StateChangedAsync(r._oldState, r._newState);

                return new StateChangedResponse();
            });


        }

        public Task StartAsync(string connectionString, string userName, string password)
        {
            var startRequest = new StartRequest()
            {
                connectionString = connectionString,
                userName = userName,
                password = password
            };

            return SendRequest<ConnectResponse>(startRequest);
        }

        public Task StopAsync()
        {
            var stopRequest = new StopRequest()
            {
            };

            return SendRequest<DisconnectResponse>(stopRequest);
        }

        public void SetCallback(IRfcServerCallbackAsync rfcServerCallback)
        {
            _rfcServerCallback = rfcServerCallback;
        }
    }
}
