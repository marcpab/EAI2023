using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.General.Tasks
{
    public class AsyncManualResetEvent
    {
        private static readonly Task _trueCompletedTask = Task.FromResult(true);

        private bool _isSignaled;
        private object _sync = new object();
        private TaskCompletionSource<bool> _waitTask;

        public Task WaitAsync()
        {
            lock (_sync)
                if (!_isSignaled)
                    return CreateWaitTask();
                else
                    return _trueCompletedTask;
        }

        public void Set()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (_sync)
            {
                if (_waitTask != null)
                    toRelease = GetWaitTask();

                SetSignaled();
            }

            ReleaseWait(toRelease);
        }

        public void Reset()
        {
            lock (_sync)
                ResetSignaled();
        }

        private void SetSignaled()
        {
            _isSignaled = true;
        }

        private void ResetSignaled()
        {
            _isSignaled = false;
        }

        private Task CreateWaitTask()
        {
            if(_waitTask == null)
                _waitTask = new TaskCompletionSource<bool>();

            return _waitTask.Task;
        }

        private TaskCompletionSource<bool> GetWaitTask()
        {
            var waitTask = _waitTask;

            _waitTask = null;

            return waitTask;
        }

        private static void ReleaseWait(TaskCompletionSource<bool> toRelease)
        {
            toRelease?.SetResult(true);
        }
    }
}
