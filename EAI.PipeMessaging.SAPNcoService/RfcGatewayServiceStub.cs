using EAI.PipeMessaging.Ping;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using EAI.SAPNco.Model;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public class RfcGatewayServiceStub : PipeObject, IRfcGatewayService
    {
        public static async Task<IRfcGatewayService> CreateObjectAsync(string pipeName = null)
        {
            var stub = new RfcGatewayServiceStub();

            await stub.CreateRemoteInstance<RfcGatewayServiceProxy>(pipeName);

            return stub;
        }

        private IRfcServerCallbackAsync _rfcServerCallback;

        private RfcGatewayServiceStub()
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

        public Task ConnectAsync(string connectionString, string userName, string password)
        {
            var connectRequest = new ConnectRequest()
            {
                connectionString = connectionString,
                userName = userName,
                password = password
            };

            return SendRequest<ConnectResponse>(connectRequest);
        }

        public Task StartServerAsync()
        {
            var startRequest = new StartServerRequest()
            {
            };

            return SendRequest<StartServerResponse>(startRequest);
        }

        public Task StopServerAsync()
        {
            var stopRequest = new StopServerRequest()
            {
            };

            return SendRequest<StopServerResponse>(stopRequest);
        }

        public Task DisconnectAsync()
        {
            var disconnectRequest = new DisconnectRequest()
            {
            };

            return SendRequest<DisconnectResponse>(disconnectRequest);
        }

        public async Task<string> GetJRfcSchemaAsync(string functionName)
        {
            var getJRfcSchemaRequest = new GetJRfcSchemaRequest()
            {
                functionName = functionName
            };

            var getJRfcSchemaResponse = await SendRequest<GetJRfcSchemaResponse>(getJRfcSchemaRequest);

            return getJRfcSchemaResponse._ret;
        }

        public async Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string functionName)
        {
            var getRfcFunctionMetadataRequest = new GetRfcFunctionMetadataRequest()
            {
                functionName = functionName
            };

            var getRfcFunctionMetadataResponse = await SendRequest<GetRfcFunctionMetadataResponse>(getRfcFunctionMetadataRequest);

            return getRfcFunctionMetadataResponse._ret;
        }

        public Task RfcPingAsync()
        {
            var rfcPingRequest = new RfcPingRequest()
            {
            };

            return SendRequest<RfcPingResponse>(rfcPingRequest);
        }

        public async Task<string> RunJRfcRequestAsync(string jRfcRequestMessage, bool autoCommit)
        {
            var runJRfcRequest = new RunJRfcRequest()
            {
                jRfcRequestMessage = jRfcRequestMessage,
                autoCommit = autoCommit
            };

            var runJRfcResponse = await SendRequest<RunJRfcResponse>(runJRfcRequest);

            return runJRfcResponse._ret;
        }

        public void SetServerCallback(IRfcServerCallbackAsync rfcServerCallback)
        {
            _rfcServerCallback = rfcServerCallback;
        }
    }
}
