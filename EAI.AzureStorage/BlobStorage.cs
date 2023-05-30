using Azure.Storage.Blobs;
using EAI.General.Cache;
using EAI.General.Storage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EAI.AzureStorage
{
    public class BlobStorage : IBlobStorage
    {
        public string ConnectionString { get; set; }
        public string RootPath { get; set; }

        public async Task<bool> ExistsAsync(string blobName)
        {
            var blob = await GetBlobAsync(blobName);

            return await blob.ExistsAsync();
        }
         
        public async Task<string> GetBlobAsStringAsync(string blobName)
        {
            using(var stream = await GetBlobAsStreamAsync(blobName))
            using(var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }

        public async Task<Stream> GetBlobAsStreamAsync(string blobName)
        {
            var blob = await GetBlobAsync(blobName);

            var stream = new MemoryStream();
            var response = await blob.DownloadAsync();

            await response.Value.Content.CopyToAsync(stream);

            stream.Position = 0;

            return stream;
        }

        public async Task SaveBlobAsync(string blobName, string content)
        {
            using(var stream = new MemoryStream())
            using(var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(content);
                await writer.FlushAsync();

                stream.Position = 0;

                await SaveBlobAsync(blobName, stream);
            }
        }

        public async Task SaveBlobAsync(string blobName, Stream stream)
        {
            var blob = await GetBlobAsync(blobName);

            await blob.UploadAsync(stream);
        }

        public async Task DeleteAsync(string blobName)
        {
            var blob = await GetBlobAsync(blobName);

            await blob.DeleteIfExistsAsync();
        }

        private string GetFullPath(string blobName)
        {
            if (string.IsNullOrEmpty(blobName))
                return RootPath;

            if (blobName.StartsWith("/"))
                return blobName;

            if (string.IsNullOrEmpty(RootPath))
                return blobName;

            if (RootPath.EndsWith("/"))
                return $"{RootPath}{blobName}";

            return $"{RootPath}/{blobName}";
        }

        private string GetContainerName(string fullPath)
        {
            return fullPath.Split('/').FirstOrDefault()?.ToLower();
        }

        private string GetBlobPath(string fullPath)
        {
            var parts = fullPath.Split('/');

            if (parts.Length <= 1)
                return null;

            return string.Join("/", parts.Skip(1));
        }

        private async Task<BlobClient> GetBlobAsync(string blobName)
        {
            var fullPath = GetFullPath(blobName);
            var containerName = GetContainerName(fullPath);
            var blobPath = GetBlobPath(fullPath);

            var client = await GetBlobContainerAsync(containerName);

            return client.GetBlobClient(blobPath);
        }

        private async Task<BlobContainerClient> GetBlobContainerAsync(string containeName)
        {
            return await ResourceCache<BlobContainerClient>.GetResourceAsync(
                        $"{ConnectionString}-{containeName}", 
                        async () => new ResourceCacheItem<BlobContainerClient>(await CreateBlobContainerResourceAsync(containeName))
                            {
                                ExpiresOn = DateTime.UtcNow.AddHours(8)
                            }
                        );
        }

        private async Task<BlobContainerClient> CreateBlobContainerResourceAsync(string containerName)
        {
            var client = new BlobContainerClient(ConnectionString, containerName);
            var clientResponse = await client.CreateIfNotExistsAsync();
            if(clientResponse != null)
            {
                var response = clientResponse.GetRawResponse();

#warning                 Log
            }

            return client;
        }

    }
}
