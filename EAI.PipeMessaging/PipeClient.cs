using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace EAI.PipeMessaging
{
    public class PipeClient : PipeMessaging
    {
        private CancellationTokenSource _cancellationTokenSource;

        public async Task RunAsync(string serverName, string pipeName, int pipeCount, IInstanceFactory instanceFactory, CancellationToken cancellationToken)
        {
            using (_cancellationTokenSource = new CancellationTokenSource())
            using (var ctr = cancellationToken.Register(SendShutdownMessage))
            {
                Setup(
                    new InstanceManager(),
                    new RequestManager(),
                    instanceFactory);

                CreatePipes(pipeName, pipeCount);

                await Task.WhenAll(Pipes.Select(p => Task.Run(() => p.ClientTaskAsync(serverName, MessageReceivedAsync, _cancellationTokenSource.Token))));
            }
        }

        protected void SendShutdownMessage()
        {
            var shutdownMessage = new PipeMessage
            {
                _action = PipeActionEnum.shutdown
            };

            SendMessage(shutdownMessage);

            Task.Delay(1000).Wait();

            _cancellationTokenSource.Cancel();
        }
        
        protected override void Shutdown()
        {
        }
    }
}
