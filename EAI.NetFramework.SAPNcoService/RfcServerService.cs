using EAI.NetFramework.SAPNco;
using EAI.PipeMessaging.SAPNcoService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNcoService
{
    public class RfcServerService : IRfcServerCallback, IRfcServerService
    {
        private RfcServer _rfcServer;
        private IRfcServerCallbackAsync _rfcServerCallback;

        public void Start(string connectionString, string userName, string password)
        {
            _rfcServer = new RfcServer()
            {
                ConnectionString = connectionString,
                UserName = userName,
                Password = password
            };

            _rfcServer.Start(this);
        }

        public void Stop()
        {
            _rfcServer.Stop();
        }


        public Task StartAsync(string connectionString, string userName, string password)
        {
            return Task.Run(() => Start(connectionString, userName, password));
        }

        public Task StopAsync()
        {
            return Task.Run(Stop);
        }

        public void SetCallback(IRfcServerCallbackAsync rfcServerCallback)
        {
            _rfcServerCallback = rfcServerCallback;
        }


        public void ApplicationError(Exception error)
        {
            _rfcServerCallback.ApplicationErrorAsync(error);
        }

        public void InvokeFunction(string functionName, JObject function)
        {
            _rfcServerCallback.InvokeFunctionAsync(functionName, function);
        }

        public void ServerError(Exception error)
        {
            _rfcServerCallback.ServerErrorAsync(error);
        }

        public void StateChanged(RfcServerState oldState, RfcServerState newState)
        {
            _rfcServerCallback.StateChangedAsync((RfcServerStateEnum)oldState, (RfcServerStateEnum)newState);
        }
    }
}
