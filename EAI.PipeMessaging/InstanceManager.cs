using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.PipeMessaging
{
    public class InstanceManager
    {
        private Dictionary<Guid, PipeObject> _instanceMap = new Dictionary<Guid, PipeObject>();

        internal PipeObject GetInstance(Guid instanceId)
        {
            lock(_instanceMap)
                return _instanceMap[instanceId];
        }

        internal void RegisterInstance(PipeObject instance)
        {
            lock(_instanceMap)
                _instanceMap.Add(instance.InstanceId, instance);
        }

        internal PipeObject RemoveInstance(Guid instanceId)
        {
            lock (_instanceMap)
            {
                var instance = _instanceMap[instanceId];

                _instanceMap.Remove(instanceId);

                return instance;
            }
        }

    }
}
