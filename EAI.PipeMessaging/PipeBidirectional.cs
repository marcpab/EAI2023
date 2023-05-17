using EAI.General.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.PipeMessaging
{
    public class PipeBidirectional
    {
        private string _pipeName;
        private int _pipeCount;

        private AsyncAutoResetEvent _sendMessageEvent = new AsyncAutoResetEvent();
        private Queue<PipeMessage> _sendMessageQueue = new Queue<PipeMessage>();

        private AsyncAutoResetEvent _receiveMessageEvent = new AsyncAutoResetEvent();
        private Queue<PipeMessage> _receiveMessageQueue = new Queue<PipeMessage>();

        private Encoding _encoding = Encoding.UTF8;

        public string PipeName { get => _pipeName; set => _pipeName = value; }
        public int PipeCount { get => _pipeCount; set => _pipeCount = value; }

        public async Task ServerTaskAsync(Func<PipeMessage, Task> receiveMessageCallback, CancellationToken cancellationToken)
        {
            using (var serverPipe = new NamedPipeServerStream(_pipeName, PipeDirection.InOut, _pipeCount, PipeTransmissionMode.Message, PipeOptions.Asynchronous | PipeOptions.WriteThrough))
            {
                await serverPipe.WaitForConnectionAsync(cancellationToken);

                await ProcessMessages(receiveMessageCallback, serverPipe, cancellationToken);

                serverPipe.Close();
            }
        }

        public async Task ClientTaskAsync(string serverName, Func<PipeMessage, Task> receiveMessageCallback, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(serverName))
                serverName = ".";

            using (var clientPipe = new NamedPipeClientStream(serverName, _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous | PipeOptions.WriteThrough))
            {
                await clientPipe.ConnectAsync(cancellationToken);

                clientPipe.ReadMode = PipeTransmissionMode.Message;

                await ProcessMessages(receiveMessageCallback, clientPipe, cancellationToken);

                clientPipe.Close();
            }
        }

        private async Task ProcessMessages(Func<PipeMessage, Task> receiveMessageCallback, PipeStream serverPipe, CancellationToken cancellationToken)
        {
            var writeTask = SendMessagesAsync(serverPipe, cancellationToken);
            var readTask = GetMessagesAsync(serverPipe, cancellationToken);
            var publishTask = PublishMessagesAsync(receiveMessageCallback, cancellationToken);

            await Task.WhenAll(readTask, writeTask, publishTask);
        }

        private async Task SendMessagesAsync(PipeStream pipeStream, CancellationToken cancellationToken)
        {
            using (var ctr = cancellationToken.Register(() => _sendMessageEvent.Set()))
            using (var bufferStream = new MemoryStream())
            using (var writer = new StreamWriter(bufferStream, _encoding, 1024, true))
            {
                var messageQueue = new Queue<PipeMessage>();
                var serializer = JsonSerializer.CreateDefault();

                while (!cancellationToken.IsCancellationRequested)
                {
                    await _sendMessageEvent.WaitAsync();

                    lock (_sendMessageQueue)
                        while (_sendMessageQueue.Count > 0)
                            messageQueue.Enqueue(_sendMessageQueue.Dequeue());

                    while (messageQueue.Count > 0)
                    {
                        var message = messageQueue.Dequeue();

                        try
                        {
                            bufferStream.Position = 0;

                            TraceMessage("Sending message", message);

                            using (var jsonWriter = new JsonTextWriter(writer))
                                serializer.Serialize(jsonWriter, message);

                            await writer.FlushAsync();
                            await bufferStream.FlushAsync();

                            await bufferStream.FlushAsync();

                            var dataLength = (int)bufferStream.Position;
                            var lengthBytes = BitConverter.GetBytes(dataLength);

                            await pipeStream.WriteAsync(lengthBytes, 0, lengthBytes.Length, cancellationToken);

                            bufferStream.Position = 0;
                            await CopyStream(bufferStream, pipeStream, dataLength, cancellationToken);

                            await pipeStream.FlushAsync();
                            await pipeStream.FlushAsync();

                            TraceMessage("Message send", message);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public async Task PublishMessagesAsync(Func<PipeMessage, Task> receiveMessageCallback, CancellationToken cancellationToken)
        {
            using (var ctr = cancellationToken.Register(() => _receiveMessageEvent.Set()))
            {
                var messageQueue = new Queue<PipeMessage>();

                while (!cancellationToken.IsCancellationRequested)
                {
                    await _receiveMessageEvent.WaitAsync();

                    lock (_receiveMessageQueue)
                        while (_receiveMessageQueue.Count > 0)
                            messageQueue.Enqueue(_receiveMessageQueue.Dequeue());

                    while (messageQueue.Count > 0)
                        _ = receiveMessageCallback(messageQueue.Dequeue());
                }
            }
        }

        public async Task GetMessagesAsync(PipeStream pipeStream, CancellationToken cancellationToken)
        {
            using (var bufferStream = new MemoryStream())
            {
                var serializer = JsonSerializer.CreateDefault();

                while (!cancellationToken.IsCancellationRequested && pipeStream.IsConnected)
                    try
                    {
                        TraceMessage("Wait message", null);


                        var lengthBytes = new byte[4];
                        await pipeStream.ReadAsync(lengthBytes, 0, lengthBytes.Length, cancellationToken);
                        var dataLength = BitConverter.ToInt32(lengthBytes, 0);

                        bufferStream.Position = 0;
                        await CopyStream(pipeStream, bufferStream, dataLength, cancellationToken);

                        bufferStream.Position = 0;

                        using (var reader = new StreamReader(bufferStream, _encoding, false, 1024, true))
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            var message = serializer.Deserialize<PipeMessage>(jsonReader);

                            TraceMessage("Read message", message);

                            lock (_receiveMessageQueue)
                                _receiveMessageQueue.Enqueue(message);

                            _receiveMessageEvent.Set();
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
            }
        }

        public static async Task CopyStream(Stream input, Stream output, int copyBytes, CancellationToken cancellationToken)
        {
            var buffer = new byte[32768];
            while (copyBytes > 0)
            {
                var readBytes = await input.ReadAsync(buffer, 0, Math.Min(buffer.Length, copyBytes), cancellationToken);
                await output.WriteAsync(buffer, 0, readBytes);
                copyBytes -= readBytes;
            }

            await output.FlushAsync();
        }

        public void EnqueueMessage(PipeMessage message)
        {
            lock(_sendMessageQueue)
                _sendMessageQueue.Enqueue(message);

            _sendMessageEvent.Set();
        }

        private void TraceMessage(string action, object message)
        {
            Trace.WriteLine($"{action}: {JsonConvert.SerializeObject(message)}");
        }
    }
}
