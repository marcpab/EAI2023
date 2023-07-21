//using EAI.Abstraction.SAPNcoService;
using EAI.General;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.OnPrem.SAPNcoService;
using EAI.PipeMessaging.SAPNcoService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcServerService : IService, IRfcServerCallbackAsync
    {
        private SapSystem _sapSystem;
        private string _pipeName;
        private LoggerV2 _log;
        private IRfcServerService _rfcServerService;
        private ProcessContext _processContext;


        public SapSystem SapSystem { get => _sapSystem; set => _sapSystem = value; }
        public string PipeName { get => _pipeName; set => _pipeName = value; }

        public LoggerV2 Log { get => _log; set => _log = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(tcs.SetResult);

            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    _processContext = ProcessContext.GetCurrent();

                    Log?.Start<Info>(nameof(_sapSystem), _sapSystem, $"Starting service {GetType().FullName}");

                    _rfcServerService = new RfcServerServiceStub();

                    Log?.String<Info>($"Start rfc server {_sapSystem.Name}");

                    await _rfcServerService.StartAsync(
                        _sapSystem.ConnectionString, 
                        _sapSystem.UserName, 
                        _sapSystem.Password);

                    Log?.String<Info>($"Rfc server started");

                    await tcs.Task;

                    Log?.String<Info>($"Stop rfc server");

                    await _rfcServerService.StopAsync();

                    Log?.String<Info>($"Rfc server stopped");

                    Log?.Success<Info>();
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }

        public Task ApplicationErrorAsync(Exception error)
        {
            ProcessContext.Restore(_processContext);

            Log.Exception<Error>(error, $"SAP application error");

            return Task.CompletedTask;
        }

        public Task InvokeFunctionAsync(string functionName, JObject functionData)
        {
            ProcessContext.Restore(_processContext);

            using (var _ = new ProcessScope(null, null, GetType().FullName))
            {

                Log.Message<Debug>(nameof(functionData), functionData, $"Received rfc call {functionName}");

            }

            return Task.CompletedTask;
        }

        public Task ServerErrorAsync(Exception error)
        {
            ProcessContext.Restore(_processContext);

            Log.Exception<Error>(error, $"SAP server error");

            return Task.CompletedTask;
        }

        public Task StateChangedAsync(RfcServerStateEnum oldState, RfcServerStateEnum newState)
        {
            ProcessContext.Restore(_processContext);

            Log.String<Info>($"SAP server state changed {oldState} => {newState}");

            return Task.CompletedTask;
        }
    }
}