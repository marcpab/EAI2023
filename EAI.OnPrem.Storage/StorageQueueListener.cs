using EAI.General.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.OnPrem.Storage
{
    public class StorageQueueListener
    {
        private IStorageQueue _storageQueue;
        private Func<string, Task> _dequeuedMessageCallback;
        private int _maxMessages = 1;
        private TimeSpan _initialWait = new TimeSpan(0, 0, 0, 0, 250);
        private TimeSpan _maxWait = new TimeSpan(0, 0, 0, 4, 0);
        private TimeSpan _currentWait;

        public StorageQueueListener(IStorageQueue storageQueue, Func<string, Task> dequeuedMessageCallback)
        { 
            _storageQueue = storageQueue;
            _dequeuedMessageCallback = dequeuedMessageCallback;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _currentWait = _initialWait;

            while(!cancellationToken.IsCancellationRequested)
            {
                await foreach(var message in _storageQueue.DequeueAsync(_maxMessages, DequeueType.AutoComplete))
                {
                    await _dequeuedMessageCallback(message.MessageContent);
                    _currentWait = _initialWait;
                }

                await Task.Delay(_currentWait);

                _currentWait += _currentWait * 0.25;
                if (_currentWait > _maxWait)
                    _currentWait = _maxWait;
            }
        }
    }
}
