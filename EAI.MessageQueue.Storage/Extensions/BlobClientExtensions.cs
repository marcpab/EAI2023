using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

namespace EAI.MessageQueue.Storage.Extensions
{
    public static class BlobClientExtensions
    {
        public async static Task<string?> DownloadStringAsync(this BlobClient blob)
        {
            if (await blob.ExistsAsync())
            {
#pragma warning disable IDE0063
                using (var memoryStream = new MemoryStream())
                {
                    var download = await blob.DownloadAsync();
                    await download.Value.Content.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;

                    using (var reader = new StreamReader(memoryStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
#pragma warning restore IDE0063
            }

            return null;
        }

        public async static Task<T?> DownloadAsync<T>(this BlobClient blob)
        {
            if (await blob.ExistsAsync())
            {
#pragma warning disable IDE0063
                using (var memoryStream = new MemoryStream())
                {
                    var download = await blob.DownloadAsync();
                    await download.Value.Content.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;

                    using (StreamReader r = new(memoryStream))
                    using (JsonReader reader = new JsonTextReader(r))
                    {
                        return new JsonSerializer().Deserialize<T>(reader);
                    }
#pragma warning restore IDE0063
                }
            }
#pragma warning disable IDE0034
            return default(T);
#pragma warning restore IDE0034
        }

        public async static Task<bool> UploadStringAsync(this BlobClient blob, string payload, string? leaseId = null, bool overwrite = false)
        {
            if (await blob.ExistsAsync() && !overwrite)
                return false;

#pragma warning disable IDE0063
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(payload);
                writer.Flush();

                memoryStream.Position = 0;

                if (!string.IsNullOrWhiteSpace(leaseId))
                    await blob.UploadAsync(memoryStream, conditions: new BlobRequestConditions { LeaseId = leaseId });
                else
                    await blob.UploadAsync(memoryStream, true);

                return true;
            }
#pragma warning restore IDE0063
        }
    }
}
