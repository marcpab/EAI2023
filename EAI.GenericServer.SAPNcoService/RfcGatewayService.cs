using EAI.Abstraction.SAPNcoService;
using EAI.General;
using EAI.General.Tasks;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.OnPrem.SAPNcoService;
using EAI.OnPrem.Storage;
using pipe = EAI.PipeMessaging.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService.Messaging;
using EAI.SAPNco.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcGatewayService : IService, IRfcGatewayService
    {
        private static readonly JsonSerializerSettings _rfcSerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        private AsyncManualResetEvent _syncRunning = new AsyncManualResetEvent();

        private RfcServerDestination[] _rfcServerDestinations;
        private IEnumerable<SapSystem> _sapSystems;
        private string _pipeName;
        private LoggerV2 _log;
        private ProcessContext _processContext;

        public IEnumerable<SapSystem> SapSystems { get => _sapSystems; set => _sapSystems = value; }
        public string PipeName { get => _pipeName; set => _pipeName = value; }
        public RfcServerDestination[] RfcServerDestinations { get => _rfcServerDestinations; set => _rfcServerDestinations = value; }
        public LoggerV2 Log { get => _log; set => _log = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(tcs.SetResult);

            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    _processContext = ProcessContext.GetCurrent();

                    Log?.Start<Info>(null, null, $"Starting service {GetType().FullName}");

                    if (_sapSystems != null)
                        foreach (var sapSystem in _sapSystems)
                        {
                            Log?.String<Info>($"conecting sap system {sapSystem.Name}, connection string {sapSystem.ConnectionString}");

                            await sapSystem.ConnectAsync(this);

                            Log?.String<Info>($"connected sap system {sapSystem.Name}");
                        }

                    _syncRunning.Set();

                    if (_sapSystems != null)
                        foreach (var sapSystem in _sapSystems.Where(s => s.StartRfcServer))
                        {
                            Log?.String<Info>($"starting rfc server {sapSystem.Name}, connection string {sapSystem.ConnectionString}");

                            await sapSystem.StartServerAsync();

                            Log?.String<Info>($"started rfc server {sapSystem.Name}");
                        }


                    await tcs.Task;

                    if (_sapSystems != null)
                        foreach (var sapSystem in _sapSystems.Where(s => s.StartRfcServer))
                        {
                            Log?.String<Info>($"stopping rfc server {sapSystem.Name}");

                            await sapSystem.StartServerAsync();

                            Log?.String<Info>($"stopped rfc server {sapSystem.Name}");
                        }

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

        public async Task<T> CallRfcAsync<T>(string name, T jRfcRequestMessage, bool autoCommit = false)
        {
            var callRfcRequest = JsonConvert.SerializeObject(jRfcRequestMessage, _rfcSerializerSettings);

            var callRfcResponse = await CallRfcAsync(name, callRfcRequest, autoCommit);

            return JsonConvert.DeserializeObject<T>(callRfcResponse);
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

        public async Task RfcServerInvokeFunctionAsync(SapSystem sapSystem, string functionName, JObject functionData)
        {
            ProcessContext.Restore(_processContext);

            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    Log.Start<Info>(nameof(functionData), functionData, $"Received rfc call on {sapSystem.Name} {functionName}");

                    functionData = new JObject(
                            new JProperty(functionName, functionData)
                        );

                    var processContext = ProcessContext.GetCurrent();
                    processContext.SetService<IRfcGatewayService>(this);
                    processContext.SetService<ISapSystem>(sapSystem);

                    if (_rfcServerDestinations != null)
                        foreach (var destination in _rfcServerDestinations)
                            if (destination.IsMatch(functionName))
                            {
                                await destination.SendMessage(functionData);
                                break;
                            }

                    Log.Success<Info>();
                }
                catch (Exception ex)
                {
                    Log.Failed<Error>(ex);
                }
        }

        public Task RfcServerApplicationErrorAsync(SapSystem sapSystem, ExceptionData error)
        {
            ProcessContext.Restore(_processContext);

            Log.Message<Error>(nameof(error), error, $"SAP application error on {sapSystem.Name}: {error._message}");

            return Task.CompletedTask;
        }

        public Task RfcServerServerErrorAsync(SapSystem sapSystem, ExceptionData error)
        {
            ProcessContext.Restore(_processContext);

            Log.Message<Error>(nameof(error), error, $"SAP server error on {sapSystem.Name}: {error._message}");

            return Task.CompletedTask;
        }

        public Task RfcServerStateChangedAsync(SapSystem sapSystem, pipe.RfcServerStateEnum oldState, pipe.RfcServerStateEnum newState)
        {
            ProcessContext.Restore(_processContext);

            Log.String<Info>($"SAP server state changed on {sapSystem.Name}: {oldState} => {newState}");

            return Task.CompletedTask;
        }
    }
}