using EAI.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;

namespace EAI.GenericServer
{
    public class ListenerPingService : IService
    {
        private IRequestListener _listener;
        private LoggerV2 _log;

        public IRequestListener Listener { get => _listener; set => _listener = value; }

        public LoggerV2 Log { get => _log; set => _log = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            using(var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    Log?.Start<Info>(null, null, $"Starting service {GetType().FullName}");

                    await _listener.RunAsync(cancellationToken);

                    Log?.Success<Info>();
                }
                catch(Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }

        private Task<string> ProcessRequestAsync(string arg)
        {
            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {

                    Log?.Start<Info>(arg, null, $"Process request");

                    var task = Task.FromResult(arg);

                    Log?.Success<Info>();

                    return task;
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }
    }
}
