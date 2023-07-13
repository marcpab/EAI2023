using EAI.Abstraction.SAPNcoService;
using EAI.General;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.OnPrem.SAPNcoService;
using Newtonsoft.Json;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcGatewayService : IService, IRfcGatewayService
    {
        private IEnumerable<SapSystem> _sapSystems;
        private string _pipeName;
        private LoggerV2 _log;

        public IEnumerable<SapSystem> SapSystems { get => _sapSystems; set => _sapSystems = value; }
        public string PipeName { get => _pipeName; set => _pipeName = value; }

        public LoggerV2 Log { get => _log; set => _log = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(tcs.SetResult);

            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    Log?.Start<Info>(null, null, $"Starting service {GetType().FullName}");

                    if (_sapSystems != null)
                        foreach (var sapSystem in _sapSystems)
                            await sapSystem.ConnectAsync(_pipeName);

                    await tcs.Task;

                    if (_sapSystems != null)
                        foreach (var sapSystem in _sapSystems)
                            await sapSystem.DisconnectAsync();

                    Log?.Success<Info>();
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }

        }

        public Task<string> CallRfcAsync(string name, string jRfcRequestMessage, bool autoCommit = false)
        {
            using (var _ = new ProcessScope(null, null, $"{GetType().FullName}+{nameof(CallRfcAsync)}"))
                try
                {
                    Log?.Start<Info>(nameof(jRfcRequestMessage), jRfcRequestMessage, $"Calling RFC on {name}");
 
                    var sapSystem = _sapSystems
                        .Where(s => s.Name == name)
                        .FirstOrDefault();

                    if (sapSystem == null)
                        throw new EAIException($"Sap system '{name}' not defined");

                    Log?.Variable<Debug>(nameof(sapSystem.ConnectionString), sapSystem.ConnectionString);

                    return sapSystem.RunJRfcRequestAsync(jRfcRequestMessage, autoCommit);
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
            }

        public Task<string> GetJRfcSchemaAsync(string name, string functionName)
        {
            using (var _ = new ProcessScope(null, null, $"{GetType().FullName}+{nameof(GetJRfcSchemaAsync)}"))
                try
                {
                    Log?.Start<Info>(null, null, $"Request schema {functionName} on {name}");
                            var sapSystem = _sapSystems
                        .Where(s => s.Name == name)
                        .FirstOrDefault();

                    if (sapSystem == null)
                        throw new EAIException($"Sap system '{name}' not defined");

                    Log?.Variable<Debug>(nameof(sapSystem.ConnectionString), sapSystem.ConnectionString);

                    return sapSystem.GetJRfcSchemaAsync(functionName);
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }
    }
}