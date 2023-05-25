using EAI.PipeMessaging;
using System;

namespace EAI.GenericServerHost
{
    internal class InstanceFactory : IInstanceFactory
    {
        public object CreateInstance(string typeName, string assemblyName)
             => Activator.CreateInstance(assemblyName, typeName).Unwrap();
    }


}