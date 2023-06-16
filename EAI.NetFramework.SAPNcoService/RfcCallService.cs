using EAI.NetFramework.SAPNco;
using EAI.PipeMessaging.SAPNcoService;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNcoService
{ 
    public class RfcCallService : IRfcCallService
    {
        private RfcConnection _rfcConnection;

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

        public void RfcPing()
        {
            _rfcConnection.Ping();
        }

        public Transaction BeginTransaction()
        {
            return new Transaction(_rfcConnection);
        }

        public string RunJRfcRequest(string jRfcRequestMessage, Transaction transaction, bool autoCommit)
        {
            var jRfcObject = JObject.Parse(jRfcRequestMessage);

            foreach (var jRfcFunction in jRfcObject.Properties().ToArray())
            {
                var jRfcFunctionResponse = _rfcConnection.InvokeJFunction(jRfcFunction, null, true);

                jRfcFunction.AddAfterSelf(jRfcFunctionResponse);
            }

            return jRfcObject.ToString();
        }

        public string GetJRfcSchema(string functionName)
        {
            var jRfc = _rfcConnection.GetJRfcSchema(functionName);

            return jRfc.ToString();
        }

        public void Disconnect()
        {
            _rfcConnection.Disconnect();
        }

        public Task ConnectAsync(string connectionString, string userName, string password)
        {
            return Task.Run(() => Connect(connectionString, userName, password));
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
    }
}
