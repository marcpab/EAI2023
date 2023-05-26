using EAI.General;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcGatewayService : IService
    {
        private IEnumerable<SapSystem> _sapSystems;
        private string _pipeName;
        private IRequestListener _listener;

        public IEnumerable<SapSystem> SapSystems { get => _sapSystems; set => _sapSystems = value; }
        public IRequestListener Listener { get => _listener; set => _listener = value; }
        public string PipeName { get => _pipeName; set => _pipeName = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(tcs.SetResult);

            if (_sapSystems != null)
                foreach (var sapSystem in _sapSystems)
                    await sapSystem.ConnectAsync(_pipeName);

            var listenerTask = _listener.RunAsync(ProcessRequestAsync, cancellationToken);

            await tcs.Task;

            await listenerTask;

            if (_sapSystems != null)
                foreach (var sapSystem in _sapSystems)
                    await sapSystem.DisconnectAsync();
        }

        private Task<string> ProcessRequestAsync(string arg)
        {
            var callRfcRequest = JsonConvert.DeserializeObject<CallRfcRequest>(arg);

            return CallRfcAsync(callRfcRequest._name, callRfcRequest._jRfcRequestMessage);
        }

        public Task<string> CallRfcAsync(string name, string jRfcRequestMessage)
        {
            var sapSystem = _sapSystems
                .Where(s => s.Name == name)
                .FirstOrDefault();

            if (sapSystem == null)
                throw new EAIException($"Sap system '{name}' not defined");

            return sapSystem.RunJRfcRequestAsync(jRfcRequestMessage);
        }
    }

    public class CallRfcRequest
    {
        public string _name;
        public string _jRfcRequestMessage;
    }

}