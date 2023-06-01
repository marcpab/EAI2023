using EAI.Abstraction.SAPNcoService;
using EAI.General;
using EAI.OnPrem.SAPNcoService;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcGatewayService : IService, IRfcGatewayService
    {
        private IEnumerable<SapSystem> _sapSystems;
        private string _pipeName;

        public IEnumerable<SapSystem> SapSystems { get => _sapSystems; set => _sapSystems = value; }
        public string PipeName { get => _pipeName; set => _pipeName = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(tcs.SetResult);

            if (_sapSystems != null)
                foreach (var sapSystem in _sapSystems)
                    await sapSystem.ConnectAsync(_pipeName);

            await tcs.Task;

            if (_sapSystems != null)
                foreach (var sapSystem in _sapSystems)
                    await sapSystem.DisconnectAsync();
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

        public Task<string> GetJRfcSchemaAsync(string name, string functionName)
        {
            var sapSystem = _sapSystems
                .Where(s => s.Name == name)
                .FirstOrDefault();

            if (sapSystem == null)
                throw new EAIException($"Sap system '{name}' not defined");

            return sapSystem.GetJRfcSchemaAsync(functionName);
        }
    }
}