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

            var serviceHost = new ServiceHost();

            await serviceHost.InitializeAsync(typeof(Program));


            if (isService)
            {
                throw new NotImplementedException("Service support currently not implemented");
            }
            else
                await RunConsoleAsync(serviceHost);

            await _clientPipeMessagingFactory.WaitAsync();

            return 0;
        }

        private static async Task RunConsoleAsync(ServiceHost program)
        {
            using (var cancellationTokenSoucre = new CancellationTokenSource())
            {
                _clientPipeMessagingFactory.Initialize(1, cancellationTokenSoucre.Token);

                var serviceTask = program.RunAsync(cancellationTokenSoucre.Token);

                while (!cancellationTokenSoucre.Token.IsCancellationRequested)
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

                await serviceTask;
            }
        }
    }
}