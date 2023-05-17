using EAI.NetFramework.SAPNco;
using EAI.PipeMessaging.SAPNcoService;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace EAI.Framework.SAPNco
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

        public string RunJRfcRequest(string jRfcRequestMessage)
        {
            var jRfcContext = new JRfcContext();

            var jRfcObject = JObject.Parse(jRfcRequestMessage);

            jRfcContext.RunJRfcRequest(jRfcObject);

            return jRfcObject.ToString();
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

        public Task<string> RunJRfcRequestAsync(string jRfcRequestMessage)
        {
            return Task.Run(() => RunJRfcRequestAsync(jRfcRequestMessage));
        }
    }
}
