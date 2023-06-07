using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Texts
{
    public static partial class DefaultStorage
    {
        public static readonly string Signature = "eai";
        public static readonly string StorageConfigurationKey = "AzureWebJobsStorage";
        public static readonly string ConfigurationContainer = $"{Signature}-configuration";
        public static readonly string BlobLockContainer = $"{Signature}-queue-defaultmanager";
        public static readonly string QueueListContainer = $"{Signature}-queue-list";
        public static readonly string ConfigurationFile = $"{Signature}-mq.json";

#pragma warning disable CA2211
        public static Func<string, string> BlobContainer = (name) =>
        {
            return $"{Signature}-queue-{name}";
        };

        public static Func<string, string> BlobDequeueContainer = (name) =>
        {
            return $"{Signature}-queue-{name}-dequeue";
        };

        public static Func<string, string> BlobArchiveContainer = (name) =>
        {
            return $"{Signature}-queue-{name}-archive";
        };

        public static Func<string, string> FileSemaphore = (name) =>
        {
            return $"{name}.lease";
        };

        public static Func<string, string, string> QueueSchema = (target, name) =>
        {
            return $"{Signature}-q-{target}-{name}";
        };

        public static Func<string, string> ExtractContainerFromQueue = (queue) =>
        {
            return queue
                .Replace($"{Signature}-queue-", string.Empty)
                .Replace("dequeue", string.Empty);
        };
#pragma warning restore CA2211
    }
}
