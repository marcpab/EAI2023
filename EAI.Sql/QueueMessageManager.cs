using EAI.General.Storage;
using EAI.MessageQueue.SQL;
using EAI.MessageQueue.SQL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Sql
{
    public class QueueMessageManager : IQueueMessageManager 
    {
        public string ConnectionString { get; set; }

        public async IAsyncEnumerable<QueueMessage> DequeueMessagesAsync(string stage, string endpintName)
        {
            await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
                await foreach(var queueMessage in EaiDbCommands.DequeueAsync(conn,
                            stage,
                            endpintName,
                            QueueMessageStatusEnum.enqueued,
                            QueueMessageStatusEnum.timeout,
                            QueueMessageStatusEnum.processing,
                            DateTimeOffset.UtcNow
                        ))
                    yield return queueMessage;
        }

        public async Task<long> EnqueueMessageAsync(QueueMessage queueMessage)
        {
            await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
                return await EaiDbCommands.AddQueueAsync(conn,
                            queueMessage._processId,
                            queueMessage._stage,
                            queueMessage._endpointName,

                            queueMessage._id_status,

                            queueMessage._messageKey,
                            queueMessage._messageType,
                            queueMessage._messageHash,
                            queueMessage._messageContentType,
                            queueMessage._messageContent,

                            queueMessage._createdOnUTC
                        );
        }

        public async Task FailedAsync(long messageId)
        {
            await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
                await EaiDbCommands.UpdateQueueStatusAsync(conn,
                            messageId,
                            QueueMessageStatusEnum.error
                        );
        }

        public async Task SuccessAsync(long messageId)
        {
            await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
                await EaiDbCommands.UpdateQueueStatusAsync(conn,
                            messageId,
                            QueueMessageStatusEnum.success
                        );
        }
    }
}
