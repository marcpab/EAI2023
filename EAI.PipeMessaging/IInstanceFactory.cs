namespace EAI.PipeMessaging
{
    public interface IInstanceFactory
    {
        object CreateInstance(string typeName, string assemblyName);
    }
}