using EAI.NetFramework.SAPNco;
using EAI.PipeMessaging.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using EAI.SAPNco.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNcoService
{ 
    public class RfcGatewayService : IRfcGatewayService, IRfcServerCallback
    {
        private RfcConnection _rfcConnection;
        private RfcServer _rfcServer;
        private IRfcServerCallbackAsync _rfcServerCallback;

        public bool IsConnected { get => _rfcConnection != null; }

        public void Connect(string connectionString, string userName, string password)
        {
            _rfcConnection = new RfcConnection()
            {
                ConnectionString = connectionString,
                UserName = userName,
                Password = password
            };

            _rfcConnection.Connect();
        }

        public void StartServer(IRfcServerCallbackAsync rfcServerCallback)
        {
            if (rfcServerCallback == null)
                throw new ArgumentNullException(nameof(rfcServerCallback));

            ThrowNotConnected();

            _rfcServerCallback = rfcServerCallback;

            _rfcServer = new RfcServer(_rfcConnection);

            _rfcServer.Start(this);
        }

        public void RfcPing()
        {
            ThrowNotConnected();

            _rfcConnection.Ping();
        }

        public Transaction BeginTransaction()
        {
            ThrowNotConnected();

            return new Transaction(_rfcConnection);
        }

        public string RunJRfcRequest(string jRfcRequestMessage, Transaction transaction, bool autoCommit)
        {
            ThrowNotConnected();

            var jRfcObject = JObject.Parse(jRfcRequestMessage);

            foreach (var jRfcFunction in jRfcObject.Properties().ToArray())
            {
                var jRfcFunctionResponse = _rfcConnection.InvokeJFunction(jRfcFunction, null, autoCommit);

                jRfcFunction.AddAfterSelf(jRfcFunctionResponse);
            }

            return jRfcObject.ToString();
        }

        public string GetJRfcSchema(string functionName)
        {
            ThrowNotConnected();

            var jRfc = _rfcConnection.GetJRfcSchema(functionName);

            return jRfc.ToString();
        }

        public RfcFunctionMetadata GetRfcFunctionMetadata(string functionName)
        {
            ThrowNotConnected();

            var rfcMetadata = _rfcConnection.GetRfcFunctionMetadata(functionName);

            return rfcMetadata;
        }

        public void StopServer()
        {
            _rfcServer.Stop();
        }

        public void Disconnect()
        {
            ThrowNotConnected();

            _rfcConnection.Disconnect();
            _rfcConnection = null;
        }


        public void ApplicationError(Exception error)
        {
            _rfcServerCallback.ApplicationErrorAsync(ExceptionData.FromException(error));
        }

        public void InvokeFunction(string functionName, JObject function)
        {
            _rfcServerCallback.InvokeFunctionAsync(functionName, function);
        }

        public void ServerError(Exception error)
        {
            _rfcServerCallback.ServerErrorAsync(ExceptionData.FromException(error));
        }

        public void StateChanged(RfcServerState oldState, RfcServerState newState)
        {
            _rfcServerCallback.StateChangedAsync((RfcServerStateEnum)oldState, (RfcServerStateEnum)newState);
        }



        public Task ConnectAsync(string connectionString, string userName, string password)
        {
            return Task.Run(() => Connect(connectionString, userName, password));
        }

        public Task StartServerAsync(IRfcServerCallbackAsync rfcServerCallback)
        {
            return Task.Run(() => StartServer(rfcServerCallback));
        }

        public Task StopServerAsync()
        {
            return Task.Run(StopServer);
        }

        public Task DisconnectAsync()
        {
            return Task.Run(() => Disconnect());
        }

        public Task RfcPingAsync()
        {
            return Task.Run(() => RfcPing());
        }

        public Task<string> RunJRfcRequestAsync(string jRfcRequestMessage, bool autoCommit)
        {
            return Task.Run(() => RunJRfcRequest(jRfcRequestMessage, null, autoCommit));
        }

        public Task<string> GetJRfcSchemaAsync(string functionName)
        {
            return Task.Run(() => GetJRfcSchema(functionName));
        }
        public Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string functionName)
        {
            return Task.Run(() => GetRfcFunctionMetadata(functionName));
        }


        private void ThrowNotConnected()
        {
            if (!IsConnected)
                throw new Exception("not connected");
        }

    }
}
