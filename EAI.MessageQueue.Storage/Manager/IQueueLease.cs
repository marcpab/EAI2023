namespace EAI.MessageQueue.Storage.Manager
{
    public interface IQueueLease : IDisposable
    {
        string LeaseId { get; }

        // release lease
        Task<bool> ReleaseAsync();

        // renew the lease (acting as heart beat meachnism that dequeuing is still alive)
        Task<bool> RenewAsync(bool isValidCheck);
    }
}
