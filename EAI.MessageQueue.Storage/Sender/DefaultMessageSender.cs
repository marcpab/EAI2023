using Azure.Storage.Queues;
using EAI.MessageQueue.Storage.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Security;
using System.Text;

namespace EAI.MessageQueue.Storage.Sender
{
    public class DefaultMessageSender : IMessageSender
    {
        private SecureString ConnectionString { get; set; }
        private IConfiguration Configuration { get; set; }
        private ILogger Log { get; set; }
        private string CS => new NetworkCredential(string.Empty, ConnectionString).Password;

        private static string GetQueue(string queue, string messageType) 
            => EAI.Texts.DefaultStorage.QueueSchema(queue, messageType);

        public string GetSenderName(MessageItem message) 
            => GetQueue(message.GetQueue, message.MessageType);


        public DefaultMessageSender(IConfiguration configuration, ILogger log)
        {
            var cs = configuration[EAI.Texts.DefaultStorage.StorageConfigurationKey] ?? configuration[$"Values:{EAI.Texts.DefaultStorage.StorageConfigurationKey}"];
            ConnectionString = new NetworkCredential(string.Empty, cs).SecurePassword;
            Configuration = configuration;
            Log = log;
        }

        public async Task SendMessageAsync(MessageItem message)
        {
            var instance = (IMessageManager?)Activator
                .CreateInstance(message.MessageManager, new object[] { Configuration, Log })
                ?? throw new InvalidOperationException("DefaultMessageSender.SendMessageAsync cannot create instance of IMessageManager");

            var ticket = instance.GetTicket(message);
            var azQueue = GetQueue(message.GetQueue, message.MessageType);
            var client = new QueueClient(CS, azQueue);
            _ = await client.CreateIfNotExistsAsync().ConfigureAwait(false);
            var payload = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ticket)));
            var receipt = await client.SendMessageAsync(payload).ConfigureAwait(false);
            Log.LogInformation("[MQ.{Queue}] AzureQueue {AzQueue} send msg receipt {Receipt}", message.GetQueue, azQueue, receipt.Value);
        }
    }
}
