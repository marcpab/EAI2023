using System;

namespace EAI.PipeMessaging.Tests
{
    class InstanceFactory : IInstanceFactory
    {
        public object CreateInstance(string typeName, string assemblyName)
             => Activator.CreateInstance(assemblyName, typeName).Unwrap();
    }


}