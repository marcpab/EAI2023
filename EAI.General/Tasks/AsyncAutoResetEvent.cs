using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.General.Tasks
{
    public class AsyncAutoResetEvent
    {
        private static readonly Task _trueCompletedTask = Task.FromResult(true);

        private bool _isSignaled;
        private readonly Queue<TaskCompletionSource<bool>> _waitTasks = new Queue<TaskCompletionSource<bool>>();

        public Task WaitAsync()
        {
            lock (_waitTasks)
                if (!_isSignaled)
                    return EnqueueWaitTask();
                else
                    return ResetSignaled();
        }

        public void Set()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (_waitTasks)
                if (_waitTasks.Count > 0)
                    toRelease = DequeueWaitTask();
                else if (!_isSignaled)
                    SetSignaled();

            ReleaseWait(toRelease);
        }

        private void SetSignaled()
        {
            _isSignaled = true;
        }

        private Task ResetSignaled()
        {
            _isSignaled = false;
            return _trueCompletedTask;
        }

        private Task EnqueueWaitTask()
        {
            var tcs = new TaskCompletionSource<bool>();
            _waitTasks.Enqueue(tcs);
            return tcs.Task;
        }

        private TaskCompletionSource<bool> DequeueWaitTask()
        {
            return _waitTasks.Dequeue();
        }

        private static void ReleaseWait(TaskCompletionSource<bool> toRelease)
        {
            toRelease?.SetResult(true);
        }
    }
}
