using EAI.General;

namespace EAI.General
{
    public interface IServiceRequestDispatcher
    {
        void Initialize(IRequestListener requestListener);
    }
}