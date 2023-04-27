using System.Threading.Tasks;

namespace EAI.Rest.Fluent
{
    public interface IFluentStreamResponse<T>
    {
        Task<T> ExecuteAsync();
    }
}