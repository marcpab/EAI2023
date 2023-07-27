using EAI.Abstraction.SAPNcoService;
using EAI.General;
using EAI.General.Tasks;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.OnPrem.SAPNcoService;
using EAI.SAPNco.Model;
using Newtonsoft.Json;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcGatewayService : IService, IRfcGatewayService
    {
        private IEnumerable<SapSystem> _sapSystems;
        private string _pipeName;
        private LoggerV2 _log;
        private AsyncManualResetEvent _syncRunning = new AsyncManualResetEvent();
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
                        {
                            Log?.String<Info>($"conecting sap system {sapSystem.Name}, connection string {sapSystem.ConnectionString}");

                            await sapSystem.ConnectAsync(_pipeName);

                            Log?.String<Info>($"connected sap system {sapSystem.Name}");
                        }

                    _syncRunning.Set();

                    await tcs.Task;

                    _syncRunning.Reset();

                    if (_sapSystems != null)
                        foreach (var sapSystem in _sapSystems)
                        {
                            Log?.String<Info>($"disconecting sap system {sapSystem.Name}");

                            await sapSystem.DisconnectAsync();

                            Log?.String<Info>($"disconnected sap system {sapSystem.Name}");
                        }

                    Log?.Success<Info>();
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }

        }

        public async Task<string> CallRfcAsync(string name, string jRfcRequestMessage, bool autoCommit = false)
        {
            await _syncRunning.WaitAsync();

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

                    var result = await sapSystem.RunJRfcRequestAsync(jRfcRequestMessage, autoCommit);

                    Log?.Success<Info>();

                    return result;
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
            }

        public async Task<string> GetJRfcSchemaAsync(string name, string functionName)
        {
            await _syncRunning.WaitAsync();

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

                    var result = await sapSystem.GetJRfcSchemaAsync(functionName);

                    Log?.Success<Info>();

                    return result;
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }

        public async Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string name, string functionName)
        {
            await _syncRunning.WaitAsync();

            using (var _ = new ProcessScope(null, null, $"{GetType().FullName}+{nameof(GetRfcFunctionMetadataAsync)}"))
                try
                {
                    Log?.Start<Info>(null, null, $"Request metadata {functionName} on {name}");
                            var sapSystem = _sapSystems
                        .Where(s => s.Name == name)
                        .FirstOrDefault();

                    if (sapSystem == null)
                        throw new EAIException($"Sap system '{name}' not defined");

                    Log?.Variable<Debug>(nameof(sapSystem.ConnectionString), sapSystem.ConnectionString);

                    var result = await sapSystem.GetRfcFunctionMetadataAsync(functionName);

                    Log?.Success<Info>();

                    return result;
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }
    }
}