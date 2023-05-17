using System;
using EAI.PipeMessaging;

namespace EAI.NetFrameworkHost
{
    class InstanceFactory : IInstanceFactory
    {
        public object CreateInstance(string typeName, string assemblyName)
                => Activator.CreateInstance(assemblyName, typeName).Unwrap();
    }
}
