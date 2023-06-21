using Xunit;
using EAI.AzureStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.AzureStorage.Tests
{
    public class BlobStorageTests
    {
        [Fact()]
        public async Task SaveBlobAsyncSimpleTestTest()
        {
            var blobName = "StringBlob";
            var blobContent = "Hello World!";
            var rootPath = "TestContainer";
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=stmodulteampandidev;AccountKey=w/7Ci2t5rJDU94Wc9VjEze9ezMl+nXLMVjmalsqhtsapQo4FIWat1GEe+ge8PwKN2/MGP1N2ZKTz+AStHyAzPg==;EndpointSuffix=core.windows.net";

            try
            {

                var blobStorage = new BlobStorage();

                blobStorage.ConnectionString = connectionString;
                blobStorage.RootPath = rootPath;

                await blobStorage.SaveBlobAsync(blobName, blobContent);

                var readBlobContent = await blobStorage.GetBlobAsStringAsync(blobName);

                Assert.Equal(blobContent, readBlobContent);


                await blobStorage.DeleteAsync(blobName);

            }
            catch(Exception ex)
            {
                throw;
            }

        }

        [Fact()]
        public async Task SaveBlobAsyncFolderTestTest()
        {
            var blobName = "Folder/StringBlob";
            var blobContent = "Hello World!";
            var rootPath = "TestContainer";
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=stmodulteampandidev;AccountKey=w/7Ci2t5rJDU94Wc9VjEze9ezMl+nXLMVjmalsqhtsapQo4FIWat1GEe+ge8PwKN2/MGP1N2ZKTz+AStHyAzPg==;EndpointSuffix=core.windows.net";

            try
            {

                var blobStorage = new BlobStorage();

                blobStorage.ConnectionString = connectionString;
                blobStorage.RootPath = rootPath;

                await blobStorage.SaveBlobAsync(blobName, blobContent);

                var readBlobContent = await blobStorage.GetBlobAsStringAsync(blobName);

                Assert.Equal(blobContent, readBlobContent);


                await blobStorage.DeleteAsync(blobName);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [Fact()]
        public async Task SaveBlobAsyncFullPathTestTest()
        {
            var blobName = "TestContainer/Folder/StringBlob";
            var blobContent = "Hello World!";
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=stmodulteampandidev;AccountKey=w/7Ci2t5rJDU94Wc9VjEze9ezMl+nXLMVjmalsqhtsapQo4FIWat1GEe+ge8PwKN2/MGP1N2ZKTz+AStHyAzPg==;EndpointSuffix=core.windows.net";

            try
            {

                var blobStorage = new BlobStorage();

                blobStorage.ConnectionString = connectionString;

                await blobStorage.SaveBlobAsync(blobName, blobContent);

                var readBlobContent = await blobStorage.GetBlobAsStringAsync(blobName);

                Assert.Equal(blobContent, readBlobContent);


                await blobStorage.DeleteAsync(blobName);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}