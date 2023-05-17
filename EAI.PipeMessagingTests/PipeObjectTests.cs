using Xunit;
using EAI.PipeMessaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using EAI.PipeMessaging.Ping;

namespace EAI.PipeMessaging.Tests
{
    public class PipeObjectTests
    {

        private Aes _aes;
        private int _serverMessages;
        private int _clientMessages;

        private int _receivedServerMessages;
        private int _receivedClientMessages;


        [Fact()]
        public async Task SendMessageAsyncTest()
        {
            PipeMessaging.DefaultPipeName = "TestPipe";
            var pipeCount = 1;

            try
            {
                using (var serverCts = new CancellationTokenSource())
                using (var clientCts = new CancellationTokenSource())
                {
                    var instanceFactory = new InstanceFactory();

                    var pipeServer = new PipeServer();
                    var serverTask = pipeServer.RunAsync(PipeMessaging.DefaultPipeName, pipeCount, instanceFactory, serverCts.Token);

                    var factory = new ClientPipeMessagingFactory();
                    factory.Initialize(pipeCount, instanceFactory, clientCts.Token);
                    PipeMessagingFactory.Instance = factory;

                    using (var pingService = await PingServiceStub.CreateObjectAsync())
                    {
                        var pingRequest = "Request";

                        string pingEventValue = null;

                        pingService.PingBackEvent = (s) => { pingEventValue = s; return Task.CompletedTask; };

                        var pingResponse = await pingService.PingAsync(pingRequest);
                        Assert.Equal(pingRequest, pingResponse);
                        Assert.Equal(pingRequest, pingEventValue);
                    }

                    clientCts.Cancel();

                    await Task.WhenAll(serverTask, factory.ClientTask);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [Fact()]
        public async Task MultiSendMessageAsyncTest()
        {
            PipeMessaging.DefaultPipeName = "TestPipe";
            var pipeCount = 1;

            try
            {
                using (var serverCts = new CancellationTokenSource())
                using (var clientCts = new CancellationTokenSource())
                {
                    var instanceFactory = new InstanceFactory();

                    var pipeServer = new PipeServer();
                    var serverTask = pipeServer.RunAsync(PipeMessaging.DefaultPipeName, pipeCount, instanceFactory, serverCts.Token);

                    var factory = new ClientPipeMessagingFactory();
                    factory.Initialize(pipeCount, instanceFactory, clientCts.Token);
                    PipeMessagingFactory.Instance = factory;

                    for (int i = 0; i < 100; i++)
                        using (var pingService = await PingServiceStub.CreateObjectAsync())
                        {
                            var pingRequest = $"Request {i}";

                            string pingEventValue = null;

                            pingService.PingBackEvent = (s) => { pingEventValue = s; return Task.CompletedTask; };

                            var pingResponse = await pingService.PingAsync(pingRequest);
                            Assert.Equal(pingRequest, pingResponse);
                            Assert.Equal(pingRequest, pingEventValue);
                        }

                    clientCts.Cancel();

                    await Task.WhenAll(serverTask, factory.ClientTask);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [Fact()]
        public async Task ParallelSendMessageAsyncTest()
        {
            PipeMessaging.DefaultPipeName = "TestPipe";
            var pipeCount = 1;

            try
            {
                using (var serverCts = new CancellationTokenSource())
                using (var clientCts = new CancellationTokenSource())
                {
                    var instanceFactory = new InstanceFactory();

                    var pipeServer = new PipeServer();
                    var serverTask = pipeServer.RunAsync(PipeMessaging.DefaultPipeName, pipeCount, instanceFactory, serverCts.Token);

                    var factory = new ClientPipeMessagingFactory();
                    factory.Initialize(pipeCount, instanceFactory, clientCts.Token);
                    PipeMessagingFactory.Instance = factory;

                    var taskList = new List<Task>();

                    for (int i = 0; i < 100; i++)
                        taskList.Add(Task.Run(async () =>
                        {
                            using (var pingService = await PingServiceStub.CreateObjectAsync())
                            {
                                var pingRequest = $"Request {i}";

                                string pingEventValue = null;

                                pingService.PingBackEvent = (s) => { pingEventValue = s; return Task.CompletedTask; };

                                var pingResponse = await pingService.PingAsync(pingRequest);
                                Assert.Equal(pingRequest, pingResponse);
                                Assert.Equal(pingRequest, pingEventValue);
                            }
                        }));

                    await Task.WhenAll(taskList);

                    clientCts.Cancel();

                    await Task.WhenAll(serverTask, factory.ClientTask);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [Fact()]
        public async Task ParallelMultiPipeSendMessageAsyncTest()
        {
            PipeMessaging.DefaultPipeName = "TestPipe";
            var pipeCount = 4;

            try
            {
                using (var serverCts = new CancellationTokenSource())
                using (var clientCts = new CancellationTokenSource())
                {
                    var instanceFactory = new InstanceFactory();

                    var pipeServer = new PipeServer();
                    var serverTask = pipeServer.RunAsync(PipeMessaging.DefaultPipeName, pipeCount, instanceFactory, serverCts.Token);

                    var factory = new ClientPipeMessagingFactory();
                    factory.Initialize(pipeCount, instanceFactory, clientCts.Token);
                    PipeMessagingFactory.Instance = factory;

                    var taskList = new List<Task>();

                    var timer = new Stopwatch();
                    timer.Start();

                    for (int i = 0; i < 1000; i++)
                        taskList.Add(Task.Run(async () =>
                        {
                            using (var pingService = await PingServiceStub.CreateObjectAsync())
                            {
                                var pingRequest = $"Request {i}";

                                string pingEventValue = null;

                                pingService.PingBackEvent = (s) => { pingEventValue = s; return Task.CompletedTask; };

                                var pingResponse = await pingService.PingAsync(pingRequest);
                                Assert.Equal(pingRequest, pingResponse);
                                Assert.Equal(pingRequest, pingEventValue);
                            }
                        }));

                    await Task.WhenAll(taskList);

                    timer.Stop();

                    clientCts.Cancel();

                    await Task.WhenAll(serverTask, factory.ClientTask);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }


    class InstanceFactory : IInstanceFactory
    {
        public object CreateInstance(string typeName, string assemblyName)
             => Activator.CreateInstance(assemblyName, typeName).Unwrap();
    }


}