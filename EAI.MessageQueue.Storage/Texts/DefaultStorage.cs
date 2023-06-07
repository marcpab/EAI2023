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

        public static Func<string, string> BlobContainer = (name) => $"{Signature}-queue-{name}";
        public static Func<string, string> BlobDequeueContainer = (name) => $"{Signature}-queue-{name}-dequeue";
        public static Func<string, string> BlobArchiveContainer = (name) => $"{Signature}-queue-{name}-archive";
        public static Func<string, string> FileSemaphore = (name) => $"{name}.lease";
        public static Func<string, string, string> QueueSchema = (target, name) => $"{Signature}-q-{target}-{name}";
        public static Func<string, string> ExtractContainerFromQueue = (queue) => queue.Replace($"{Signature}-queue-", string.Empty).Replace("dequeue", string.Empty);
    }
}
