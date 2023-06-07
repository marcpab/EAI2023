using EAI.MessageQueue.Storage.Manager;

namespace EAI.MessageQueue.Storage
{
    public class MessageObject : IDisposable
    {
        public IQueueLease? Lease { get; set; }

        private Dictionary<MessageObjectId, object> ControlFlags { get; set; } 
            = new Dictionary<MessageObjectId, object>();
        private bool Disposed { get; set; } = false;

        public MessageObject()
        {
        }

        public object? GetMessage(MessageObjectId id)
        {
            if (ControlFlags.ContainsKey(id))
                return ControlFlags[id];

            return null;
        }

        public void SetMessage(MessageObjectId id, object value)
        {
            if (ControlFlags.ContainsKey(id))
                ControlFlags[id] = value;
            else
                ControlFlags.Add(id, value);
        }

        private void Cleanup()
        {
            ControlFlags.Clear();
            Disposed = true;
        }

        public void Dispose()
        {
            while (!Disposed)
                Cleanup();

            GC.SuppressFinalize(this);
        }
    }
}
