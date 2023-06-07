namespace EAI.MessageQueue.Storage.Manager
{
    public enum DequeueStatus
    {
        Finished,
        MaxTicketsAtStart,
        MaxTicketsAtEnd,
        ContainerMissing,
        RenewLeaseFailed,
        NoLock,
        None,
        OK
    }
}
