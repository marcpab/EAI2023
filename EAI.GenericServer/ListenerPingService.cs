using EAI.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EAI.GenericServer
{
    public class ListenerPingService : IService
    {
        private IRequestListener _listener;

        public IRequestListener Listener { get => _listener; set => _listener = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await _listener.RunAsync(cancellationToken);
        }

        private Task<string> ProcessRequestAsync(string arg)
        {
            return Task.FromResult(arg);
        }
    }
}
