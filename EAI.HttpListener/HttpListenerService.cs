using EAI.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using EAI.LoggingV2.Levels;
using EAI.LoggingV2;
using Net = System.Net;
using System.Net;
using System.Runtime;
using System.Collections.Specialized;

namespace EAI.HttpListener
{
    public class HttpListenerService : IService
    {
        private LoggerV2 _log;
        private string _listenUri;
        private IHttpListenerAuth _auth;
        private HttpListenerDestination[] _httpListenerDestinations;
        private ProcessContext _processContext;

        public LoggerV2 Log { get => _log; set => _log = value; }
        public string ListenUri { get => _listenUri; set => _listenUri = value; }
        public HttpListenerDestination[] HttpListenerDestinations { get => _httpListenerDestinations; set => _httpListenerDestinations = value; }
        public IHttpListenerAuth Auth { get => _auth; set => _auth = value; }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource();
            cancellationToken.Register(tcs.SetResult);

            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    _processContext = ProcessContext.GetCurrent();

                    Log?.Start<Info>(null, null, $"Starting service {GetType().FullName}");

                    ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

                    using (var listener = new Net.HttpListener())
                    {
                        listener.Prefixes.Add(_listenUri);
                        listener.IgnoreWriteExceptions = true;
                        listener.Start();

                        while (!cancellationToken.IsCancellationRequested)
                        {
                            var ctx = await listener.GetContextAsync();

                            Task.Run(CreateHandler(ctx));

                        }

                        await tcs.Task;

                        listener.Stop();
                    }

                    Log?.Success<Info>();
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }

        private Func<Task> CreateHandler(HttpListenerContext ctx)
        {
            var requestHandler = new RequestHandler(_log, _listenUri, _httpListenerDestinations, _auth, ctx);

            return requestHandler.ProcessRequestAsync;
        }



    }
}
