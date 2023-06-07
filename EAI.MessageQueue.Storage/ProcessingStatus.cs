namespace EAI.MessageQueue.Storage
{
    public enum ProcessingStatus
    {
        New = 0,
        Processed = 10,
        Finished = 51,
        ProcessingTimeout = 52,
        FinishedButNotInQueue = 98,
        FinishedWithErrors = 99
    }
}
