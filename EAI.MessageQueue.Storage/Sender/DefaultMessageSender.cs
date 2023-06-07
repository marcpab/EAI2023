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
        //private static readonly string _QueueSchema = "rdl-q-{0}-{1}";
        private SecureString ConnectionString { get; set; }
        private IConfiguration _configuration { get; set; }
        private ILogger _log { get; set; }
        private string _cs => new NetworkCredential(string.Empty, ConnectionString).Password;

        private string getQueue(string queue, string messageType) 
            => EAI.Texts.DefaultStorage.QueueSchema(queue, messageType);

        public string GetSenderName(MessageItem message) 
            => getQueue(message.GetQueue, message.MessageType);


        public DefaultMessageSender(IConfiguration configuration, ILogger log)
        {
            var cs = configuration[EAI.Texts.DefaultStorage.StorageConfigurationKey] ?? configuration[$"Values:{EAI.Texts.DefaultStorage.StorageConfigurationKey}"];
            ConnectionString = new NetworkCredential(string.Empty, cs).SecurePassword;
            _configuration = configuration;
            _log = log;
        }

        public async Task SendMessageAsync(MessageItem message)
        {
            var instance = (IMessageManager?)Activator
                .CreateInstance(message.MessageManager, new object[] { _configuration, _log });

            if(instance == null) 
            { 
                throw new InvalidOperationException("DefaultMessageSender.SendMessageAsync cannot create instance of IMessageManager");
            }

            var ticket = instance.GetTicket(message);
            var azQueue = getQueue(message.GetQueue, message.MessageType);
            var client = new QueueClient(_cs, azQueue);
            _ = await client.CreateIfNotExistsAsync().ConfigureAwait(false);
            var payload = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ticket)));
            var receipt = await client.SendMessageAsync(payload).ConfigureAwait(false);
            _log.LogInformation($"[MQ.{message.GetQueue}] AzureQueue {azQueue} send msg receipt {receipt.Value}");
        }
    }
}
