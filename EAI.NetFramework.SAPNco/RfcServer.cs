using NCo = SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using Newtonsoft.Json.Linq;

namespace EAI.NetFramework.SAPNco
{
    public class RfcServer
    {
        private static Dictionary<NCo.RfcServer, RfcServer> _rfcServerMap = new Dictionary<NCo.RfcServer, RfcServer>();

        private static void AddServer(RfcServer rfcServer)
        {
            _rfcServerMap.Add(rfcServer._rfcServer, rfcServer);
        }

        private static RfcServer GetServer(NCo.RfcServer rfcServer)
        {
            return _rfcServerMap[rfcServer];
        }

        private static void RemoveServer(RfcServer rfcServer)
        {
            _rfcServerMap.Remove(rfcServer._rfcServer);
        }


        private RfcConnection _rfcConnection;
        private NCo.RfcDestination _rfcDestination;
        private NCo.RfcServer _rfcServer;

        private string _connectionString;
        private string _userName;
        private string _password;

        public string ConnectionString { get => _connectionString; set => _connectionString = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }

        private IRfcServerCallback _rfcServerCallback;

        public void Start(IRfcServerCallback rfcServerCallback)
        {
//            GeneralConfiguration.CPICTraceLevel = 3;

            _rfcServerCallback = rfcServerCallback;

            _rfcConnection = new RfcConnection()
            {
                ConnectionString = _connectionString,
                UserName = _userName,
                Password = _password
            };

            _rfcConnection.Connect();
            _rfcDestination = _rfcConnection.GetRfcDestination();
            _rfcServer = NCo.RfcServerManager.GetServer(_rfcDestination.Parameters, new[] { typeof(RfcServer) }, _rfcDestination.Repository);

            AddServer(this);

            _rfcServer.RfcServerApplicationError += _rfcServer_RfcServerApplicationError;
            _rfcServer.RfcServerError += _rfcServer_RfcServerError;
            _rfcServer.RfcServerStateChanged += _rfcServer_RfcServerStateChanged;

            _rfcServer.TransactionIDHandler = new RfcTransactionIdHandler();

            _rfcServer.Start();
        }

        public void Stop()
        {
            var rfcServer = _rfcServer;
            _rfcServer = null;

            if (rfcServer != null)
            {
                rfcServer.Shutdown(false);

                RemoveServer(this);
            }

            _rfcDestination = null;

            var rfcConnection = _rfcConnection;
            _rfcConnection = null;

            if (rfcConnection != null)
                rfcConnection.Disconnect();
        }

        private void _rfcServer_RfcServerApplicationError(object server, NCo.RfcServerErrorEventArgs errorEventData)
        {
            _rfcServerCallback.ApplicationError(errorEventData.Error);
        }

        private void _rfcServer_RfcServerError(object server, NCo.RfcServerErrorEventArgs errorEventData)
        {
            _rfcServerCallback.ServerError(errorEventData.Error);
        }

        private void _rfcServer_RfcServerStateChanged(object server, NCo.RfcServerStateChangedEventArgs stateEventData)
        {
            _rfcServerCallback.StateChanged((RfcServerState)stateEventData.OldState, (RfcServerState)stateEventData.NewState);
        }

        [NCo.RfcServerFunction(Default = true)]
        public static void RfcServerFunction(NCo.RfcServerContext ctx, NCo.IRfcFunction function)
        {
            var rfcServer = GetServer(ctx.Server);

            if (rfcServer != null)
            {
                var jObject = new JObject();

                JRfc.RfcDataToJson(function, jObject);

                rfcServer._rfcServerCallback.InvokeFunction(ctx.FunctionName, jObject);
            }
        }
    }

}
