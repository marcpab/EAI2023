using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Storage
{
    public class FileBlobStorage : IBlobStorage
    {
        public Task DeleteAsync(string path)
        {
            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string path)
        {
            return Task.FromResult(File.Exists(path));
        }

        public Task<Stream> GetBlobAsStreamAsync(string path)
        {
            return Task.FromResult((Stream)new FileStream(path, FileMode.Open));
        }

        public async Task<string> GetBlobAsStringAsync(string path)
        {
            using(var streamReader = new StreamReader(path))
                return await streamReader.ReadToEndAsync();
        }

        public Task SaveBlobAsync(string path, Stream stream)
        {
            return Task.FromResult((Stream)new FileStream(path, FileMode.OpenOrCreate));
        }

        public async Task SaveBlobAsync(string path, string content)
        {
            using(var streamWriter = new StreamWriter(path, false))
                await streamWriter.WriteAsync(content);
        }
    }
}
