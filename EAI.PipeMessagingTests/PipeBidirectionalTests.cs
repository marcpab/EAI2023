using Xunit;
using EAI.PipeMessaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;

namespace EAI.PipeMessaging.Tests
{
    public class PipeBidirectionalTests
    {

        private Aes _aes;
        private int _serverMessages;
        private int _clientMessages;

        private int _receivedServerMessages;
        private int _receivedClientMessages;


        [Fact()]
        public async Task PipeMessagingTest()
        {
            try
            {
                _serverMessages = 100;
                _clientMessages = 100;

                await Task.WhenAll(
                    Task.Run(ServerPartAsync),
                    Task.Run(ClientPartAsync)
                );

                Assert.Equal(_serverMessages, _receivedServerMessages);
                Assert.Equal(_clientMessages, _receivedClientMessages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task ClientPartAsync()
        {
            var cts = new CancellationTokenSource();

            var client = new EAI.PipeMessaging.PipeBidirectional();

            client.PipeName = "testpipe";
            client.PipeCount = 1;

            var waitTask = client.ClientTaskAsync(null, m => { Debug.WriteLine($"Client received message: {m._payload}"); _receivedServerMessages++; return Task.CompletedTask; }, cts.Token);

            int i = 0;

            while (i < _clientMessages)
            {
                var message = new PipeMessage { _payload = $"Client message #{i}" };

                Debug.WriteLine($"Client send message: {message._payload}");

                client.EnqueueMessage(message);

//                await Task.Delay(1000);

                i++;
            }

            await Task.Delay(1000);

            cts.Cancel();

            await waitTask;
        }

        private async Task ServerPartAsync()
        {
            var cts = new CancellationTokenSource();

            var server = new EAI.PipeMessaging.PipeBidirectional();

            server.PipeName = "testpipe";
            server.PipeCount = 1;

            var waitTask = server.ServerTaskAsync(m => { Debug.WriteLine($"Server received message: {m._payload}"); _receivedClientMessages++; return Task.CompletedTask; }, cts.Token);

            int i = 0;

            while ( i < _serverMessages )
            {
                var message = new PipeMessage { _payload = $"Server message #{i}" };

                Debug.WriteLine($"Server send message: {message._payload}");

                server.EnqueueMessage(message);

//                await Task.Delay(1000);

                i++;
            }

            await Task.Delay(1000);

            cts.Cancel();

            await waitTask;
        }
    }
}