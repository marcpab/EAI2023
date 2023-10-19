namespace EAI.MessageQueue.SQL
{
    public enum QueueMessageStatusEnum : byte
    {
        new2 = 255,
        enqueued = 0,
        processing = 10,
        success = 51,
        timeout = 52,
        error = 99
    }
}