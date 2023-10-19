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

        public IAsyncEnumerable<QueueMessage> DequeueMessages(QueueMessageStatusEnum id_status)
        {
            throw new NotImplementedException();

            //await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
            //    await foreach(var queueMessage in await EaiDbCommands.Dequeue(conn,
            //        id_status
            //            ))
            //        yield return queueMessage;
        }

        public IAsyncEnumerable<QueueMessage> DequeueMessages()
        {
            throw new NotImplementedException();
        }

        public async Task<long> EnqueueMessage(QueueMessage queueMessage)
        {
            await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
                return await EaiDbCommands.AddQueue(conn,
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
    }
}
