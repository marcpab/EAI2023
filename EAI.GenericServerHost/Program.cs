using EAI.GenericServer;
using EAI.PipeMessaging;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

namespace EAI.GenericServerHost
{
    public class Program
    {
        private const string _servicesDirName = "services";
        private const string _netFrameworkHostPath = "EAI.NetFrameworkHost\\EAI.NetFrameworkHost.exe";
        private static ClientPipeMessagingFactory _clientPipeMessagingFactory;

        static Program()
        {
            var currentDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var servicesDir = Path.Combine(currentDir, _servicesDirName);

            AssemblyLoadContext.Default.Resolving += new ServiceLoader()
            {
                ServicesDir = servicesDir
            }.AssemblyResolver;

            _clientPipeMessagingFactory = new ClientPipeMessagingFactory()
            {
                NetFrameworkHostFilePath = Path.Combine(servicesDir, _netFrameworkHostPath),
                InstanceFactory = new InstanceFactory()
            };

            PipeObjectMessagingFactory.Instance = _clientPipeMessagingFactory;
        }

        public static async Task<int> Main(string[] args)
        {
            var isService = args.Contains("-service");

            if (isService)
                WindowsService.Run(new WindowsService());
            else
                await RunConsoleAsync();

            await _clientPipeMessagingFactory.WaitAsync();

            return 0;
        }

        private static async Task RunConsoleAsync()
        {
            using (var cancellationTokenSoucre = new CancellationTokenSource())
            {
                Task serviceTask = RunAsync(cancellationTokenSoucre.Token);

                while (!cancellationTokenSoucre.Token.IsCancellationRequested)
                {
                    if (serviceTask.IsFaulted)
                        break;

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey();

                        switch (key.KeyChar)
                        {
                            case 'q':
                            case 'Q':
                            case 'x':
                            case 'X':
                                cancellationTokenSoucre.Cancel();
                                break;
                        }
                    }

                    await Task.Delay(100);
                }

                await serviceTask;
            }
        }

        public static async Task RunAsync(CancellationToken cancellationToken)
        {
            var serviceHost = new ServiceHost();

            await serviceHost.InitializeAsync(typeof(Program));

            _clientPipeMessagingFactory.Initialize(1, cancellationToken);

            await serviceHost.RunAsync(cancellationToken);
        }
    }
}