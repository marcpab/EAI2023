using EAI.PipeMessaging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.GenericServerHost
{
    internal class ClientPipeMessagingFactory : PipeObjectMessagingFactory
    {
        private int _pipeCount;
        private InstanceFactory _instanceFactory;
        private CancellationToken _cancellationToken;
        private List<Task> _clientTasks = new List<Task>();
        private string _netFrameworkHostFilePath;
        private Process? _process;

        public string NetFrameworkHostFilePath { get => _netFrameworkHostFilePath; set => _netFrameworkHostFilePath = value; }
        internal InstanceFactory InstanceFactory { get => _instanceFactory; set => _instanceFactory = value; }

        public void Initialize(int pipeCount, CancellationToken cancellationToken)
        {
            _pipeCount = pipeCount;
            _cancellationToken = cancellationToken;
        }

        public override PipeObjectMessaging CreatePipeMessaging(string pipeName)
        {
            var psi = new ProcessStartInfo(_netFrameworkHostFilePath)
            {
                FileName = _netFrameworkHostFilePath,
                Arguments = $"{pipeName} {_pipeCount}",
                UseShellExecute = true
            };

            _process = Process.Start(psi);

            Task.Delay(1000).Wait();

            var pipeClient = new PipeClient();

            _clientTasks.Add(pipeClient.RunAsync(null, pipeName, _pipeCount, _instanceFactory, _cancellationToken));

            return pipeClient;
        }

        public async Task WaitAsync()
        {
            await Task.WhenAll(_clientTasks);

            if(_process != null)
                await _process.WaitForExitAsync();
        }
    }
}