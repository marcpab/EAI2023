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
        public static readonly string StorageConfigurationKey = "AzureWebJobsStorage";
        public static readonly string ConfigurationContainer = "cosmo-configuration";
        public static readonly string BlobLockContainer = "cosmo-queue-defaultmanager";
        public static readonly string QueueListContainer = "cosmo-queue-list";

        public static Func<string, string> BlobContainer = (name) => $"cosmo-queue-{name}";
        public static Func<string, string> BlobDequeueContainer = (name) => $"cosmo-queue-{name}-dequeue";
        public static Func<string, string> BlobArchiveContainer = (name) => $"cosmo-queue-{name}-archive";
        public static Func<string, string> FileSemaphore = (name) => $"{name}.lease";
        public static Func<string, string, string> QueueSchema = (target, name) => $"rdl-q-{target}-{name}";
    }
}
