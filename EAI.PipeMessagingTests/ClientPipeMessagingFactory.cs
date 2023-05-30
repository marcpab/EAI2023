using System.Threading;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.Tests
{
    internal class ClientPipeMessagingFactory : PipeObjectMessagingFactory
    {
        private int _pipeCount;
        private InstanceFactory _instanceFactory;
        private CancellationToken _cancellationToken;
        private Task _clientTask;

        public Task ClientTask { get => _clientTask; }

        public void Initialize(int pipeCount, InstanceFactory instanceFactory, CancellationToken cancellationToken)
        {
            _pipeCount = pipeCount;
            _instanceFactory = instanceFactory;
            _cancellationToken = cancellationToken;
        }

        public override PipeObjectMessaging CreatePipeMessaging(string pipeName)
        {
            var pipeClient = new PipeClient();

            _clientTask = pipeClient.RunAsync(null, pipeName, _pipeCount, _instanceFactory, _cancellationToken);

            return pipeClient;
        }
    }
}