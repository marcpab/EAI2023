using System;
using System.Threading.Tasks;
using Xunit;

namespace EAI.AzureStorage.Tests
{
    public class StorageQueueTests
    {
        [Fact()]
        public async Task QueueManualCompleteAsyncTest()
        {
            var queueContent = "Hello World!";
            var queueName = "TestQueue";
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=stmodulteampandidev;AccountKey=w/7Ci2t5rJDU94Wc9VjEze9ezMl+nXLMVjmalsqhtsapQo4FIWat1GEe+ge8PwKN2/MGP1N2ZKTz+AStHyAzPg==;EndpointSuffix=core.windows.net";

            try
            {
                var storageQueue = new StorageQueue();

                storageQueue.ConnectionString = connectionString;
                storageQueue.StorageQueueName = queueName;

                await storageQueue.EnqueueAsync(queueContent);

                var dequeueCount = 0;
                await foreach (var readQueueContent in storageQueue.DequeueAsync(1))
                {
                    dequeueCount++;
                    Assert.Equal(queueContent, readQueueContent.MessageContent);

                    await storageQueue.CompletedAsync(readQueueContent);
                }

                Assert.Equal(1, dequeueCount);


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Fact()]
        public async Task QueueAutoCompleteAsyncTest()
        {
            var queueContent = "Hello World!";
            var queueName = "TestQueue";
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=stmodulteampandidev;AccountKey=w/7Ci2t5rJDU94Wc9VjEze9ezMl+nXLMVjmalsqhtsapQo4FIWat1GEe+ge8PwKN2/MGP1N2ZKTz+AStHyAzPg==;EndpointSuffix=core.windows.net";

            try
            {
                var storageQueue = new StorageQueue();

                storageQueue.ConnectionString = connectionString;
                storageQueue.StorageQueueName = queueName;

                await storageQueue.EnqueueAsync(queueContent);

                var dequeueCount = 0;
                await foreach (var readQueueContent in storageQueue.DequeueAsync(1))
                {
                    dequeueCount++;
                    Assert.Equal(queueContent, readQueueContent.MessageContent);
                }

                Assert.Equal(1, dequeueCount);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Fact()]
        public async Task QueueMultipleAsyncTest()
        {
            var queueContent = "Hello World!";
            var queueName = "TestQueue";
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=stmodulteampandidev;AccountKey=w/7Ci2t5rJDU94Wc9VjEze9ezMl+nXLMVjmalsqhtsapQo4FIWat1GEe+ge8PwKN2/MGP1N2ZKTz+AStHyAzPg==;EndpointSuffix=core.windows.net";

            try
            {
                var storageQueue = new StorageQueue();

                storageQueue.ConnectionString = connectionString;
                storageQueue.StorageQueueName = queueName;

                await storageQueue.EnqueueAsync(queueContent);
                await storageQueue.EnqueueAsync(queueContent);
                await storageQueue.EnqueueAsync(queueContent);
                await storageQueue.EnqueueAsync(queueContent);

                var dequeueCount = 0;
                await foreach (var readQueueContent in storageQueue.DequeueAsync(2))
                {
                    dequeueCount++;
                    Assert.Equal(queueContent, readQueueContent.MessageContent);

                    await storageQueue.CompletedAsync(readQueueContent);
                }

                Assert.Equal(2, dequeueCount);

                dequeueCount = 0;
                await foreach (var readQueueContent in storageQueue.DequeueAsync(2))
                {
                    dequeueCount++;
                    Assert.Equal(queueContent, readQueueContent.MessageContent);

                    await storageQueue.CompletedAsync(readQueueContent);
                }

                Assert.Equal(2, dequeueCount);

                dequeueCount = 0;
                await foreach (var readQueueContent in storageQueue.DequeueAsync(2))
                {
                    dequeueCount++;
                    Assert.Equal(queueContent, readQueueContent.MessageContent);

                    await storageQueue.CompletedAsync(readQueueContent);
                }

                Assert.Equal(0, dequeueCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}