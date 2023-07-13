using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Storage
{
    public class FileBlobStorage : IBlobStorage
    {
        public string RootPath { get; set; }

        public Task DeleteAsync(string path)
        {
            path = Path.Combine(RootPath, path);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string path)
        {
            path = Path.Combine(RootPath, path);

            return Task.FromResult(File.Exists(path));
        }

        public Task<Stream> GetBlobAsStreamAsync(string path)
        {
            path = Path.Combine(RootPath, path);

            return Task.FromResult((Stream)new FileStream(path, FileMode.Open));
        }

        public async Task<string> GetBlobAsStringAsync(string path)
        {
            path = Path.Combine(RootPath, path);

            using (var streamReader = new StreamReader(path))
                return await streamReader.ReadToEndAsync();
        }

        public Task SaveBlobAsync(string path, Stream stream)
        {
            path = Path.Combine(RootPath, path);

            return Task.FromResult((Stream)new FileStream(path, FileMode.OpenOrCreate));
        }

        public async Task SaveBlobAsync(string path, string content)
        {
            path = Path.Combine(RootPath, path);

            using (var streamWriter = new StreamWriter(path, false))
                await streamWriter.WriteAsync(content);
        }
    }
}
