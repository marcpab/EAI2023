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

namespace EAI
{
    public class BufferedQueue<messageT>
    {
        private AsyncAutoResetEvent _syncEvent = new AsyncAutoResetEvent();
        private Queue<messageT> _messageQueue = new Queue<messageT>();
        private List<TaskCompletionSource<object>> _flushTasks = new List<TaskCompletionSource<object>>();

        public async Task RunAsync(Func<messageT, Task> processMessageAsync, CancellationToken cancellationToken)
        {
            var messageQueue = new Queue<messageT>();

            while (!cancellationToken.IsCancellationRequested)
            {
                await _syncEvent.WaitAsync();

                lock (_messageQueue)
                    while (_messageQueue.Count > 0)
                        messageQueue.Enqueue(_messageQueue.Dequeue());

                while (messageQueue.Count > 0)
                {
                    var message = messageQueue.Dequeue();

                    await processMessageAsync(message);
                }

                lock (_messageQueue)
                    if(_messageQueue.Count == 0)
                    {
                        foreach(var flushTask in _flushTasks)
                            flushTask.SetResult(null);

                        _flushTasks.Clear();
                    }
            }
        }

        public Task FlushAsync()
        {
            lock (_messageQueue)
            {
                if(_messageQueue.Count == 0)
                    return Task.CompletedTask;

                var flushTask = new TaskCompletionSource<object>();
                _flushTasks.Add(flushTask);

                return flushTask.Task;
            }
        }

        public void EnqueueMessage(messageT message)
        {
            lock(_messageQueue)
                _messageQueue.Enqueue(message);

            _syncEvent.Set();
        }
    }
}
