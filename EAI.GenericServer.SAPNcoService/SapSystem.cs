using EAI.Abstraction.SAPNcoService;
using EAI.General;
using EAI.LoggingV2.Levels;
using pipe = EAI.PipeMessaging.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using EAI.SAPNco.Model;
using Newtonsoft.Json.Linq;

namespace EAI.GenericServer.SAPNcoService
{
    public class SapSystem : pipe.IRfcServerCallbackAsync, ISapSystem
    {
        private string _name;
        private string _connectionString;
        private string _userName;
        private string _password;
        private bool _startRfcServer;
        private RfcGatewayService _rfcGatewayService;
        private pipe.IRfcGatewayService _remoteRfcGatewayService;

        public string Name { get => _name; set => _name = value; }
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        public bool StartRfcServer { get => _startRfcServer; set => _startRfcServer = value; }

        internal async Task ConnectAsync(RfcGatewayService rfcGatewayService)
        {
            try
            {
                _rfcGatewayService = rfcGatewayService;

                _remoteRfcGatewayService = await pipe.RfcGatewayServiceStub.CreateObjectAsync(_rfcGatewayService.PipeName);

                await _remoteRfcGatewayService.ConnectAsync(
                    _connectionString,
                    _userName,
                    _password);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal Task StartServerAsync()
        {
            return _remoteRfcGatewayService.StartServerAsync(this);
        }

        internal Task<string> RunJRfcRequestAsync(string jRfcRequestMessage, bool autoCommit)
        {
            return _remoteRfcGatewayService.RunJRfcRequestAsync(jRfcRequestMessage, autoCommit);
        }

        internal async Task DisconnectAsync()
        {
            await _remoteRfcGatewayService.DisconnectAsync();

            (_remoteRfcGatewayService as IDisposable)?.Dispose();
        }

        internal Task<string> GetJRfcSchemaAsync(string functionName)
        {
            return _remoteRfcGatewayService.GetJRfcSchemaAsync(functionName);
        }
        internal Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string functionName)
        {
            return _remoteRfcGatewayService.GetRfcFunctionMetadataAsync(functionName);
        }


        public Task ApplicationErrorAsync(ExceptionData error)
        {
            return _rfcGatewayService.RfcServerApplicationErrorAsync(this, error);
        }

        public Task InvokeFunctionAsync(string functionName, JObject functionData)
        {
            return _rfcGatewayService.RfcServerInvokeFunctionAsync(this, functionName, functionData);
        }

        public Task ServerErrorAsync(ExceptionData error)
        {
            return _rfcGatewayService.RfcServerServerErrorAsync(this, error);
        }

        public Task StateChangedAsync(pipe.RfcServerStateEnum oldState, pipe.RfcServerStateEnum newState)
        {
            return _rfcGatewayService.RfcServerStateChangedAsync(this, oldState, newState);
        }
    }
}