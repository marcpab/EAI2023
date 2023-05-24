using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EAI.General.Extensions;
using EAI.PipeMessaging;

namespace EAI.NetFrameworkHost
{
    internal partial class Program
    {
        static async Task Main(string[] args)
        {
            var pipeName = args[0];
            var pipeCount = args[1].TryParseInt().Value;

            using (var serverCts = new CancellationTokenSource())
            {
                var instanceFactory = new InstanceFactory();

                var pipeServer = new PipeServer();
                await pipeServer.RunAsync(pipeName, pipeCount, instanceFactory, serverCts.Token);
            }
        }
    }
}
