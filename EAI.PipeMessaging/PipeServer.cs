using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.PipeMessaging
{
    public class PipeServer : PipeMessaging
    {
        private CancellationTokenSource _cancellationTokenSource;

        public async Task RunAsync(string pipeName, int pipeCount, IInstanceFactory instanceFactory, CancellationToken cancellationToken)
        {
            using(_cancellationTokenSource = new CancellationTokenSource())
            using(var ctr = cancellationToken.Register(_cancellationTokenSource.Cancel))
            {
                Setup(
                    new InstanceManager() , 
                    new RequestManager(), 
                    instanceFactory);

                CreatePipes(pipeName, pipeCount);

                await Task.WhenAll(Pipes.Select(p => Task.Run(() => p.ServerTaskAsync(MessageReceivedAsync, _cancellationTokenSource.Token))));
            }
        }

        protected override void Shutdown()
        {
            base.Shutdown();

            _cancellationTokenSource.Cancel();
        }

    }
}
