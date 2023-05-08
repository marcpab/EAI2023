using System.IO;
using System.Threading.Tasks;

namespace EAI.General.Storage
{
    public interface IBlobStorage
    {
        Task DeleteAsync(string path);
        Task<bool> ExistsAsync(string path);
        Task<Stream> GetBlobAsStreamAsync(string path);
        Task<string> GetBlobAsStringAsync(string path);
        Task SaveBlobAsync(string path, Stream stream);
        Task SaveBlobAsync(string path, string content);
    }
}