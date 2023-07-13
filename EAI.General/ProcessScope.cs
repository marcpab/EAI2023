using System;

namespace EAI.General
{
    public class ProcessScope : IDisposable
    {
        private readonly ProcessContext _parentContext;
        private readonly ProcessContext _context;

        public ProcessScope(string processId = null, string stage = null, string serviceName = null)
        {
            _parentContext = ProcessContext.GetCurrent();

            _context = ProcessContext.Create(processId, stage, serviceName);
        }

        public void Dispose()
        {
            ProcessContext.Restore(_parentContext);
        }
    }
}
