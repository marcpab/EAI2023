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

            await stub.CreateRemoteInstance<RfcServerServiceStub>(pipeName);

            return stub;
        }

        private IRfcServerCallbackAsync _rfcServerCallback;

        public RfcServerServiceStub()
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
            var connectRequest = new ConnectRequest()
            {
                connectionString = connectionString,
                userName = userName,
                password = password
            };

            return SendRequest<ConnectResponse>(connectRequest);
        }

        public Task StopAsync()
        {
            var disconnectRequest = new DisconnectRequest()
            {
            };

            return SendRequest<DisconnectResponse>(disconnectRequest);
        }

        public void SetCallback(IRfcServerCallbackAsync rfcServerCallback)
        {
            _rfcServerCallback = rfcServerCallback;
        }
    }
}
