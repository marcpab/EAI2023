using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.PipeMessaging
{
    public enum PipeActionEnum
    {
        createInstanceRequest,
        request,
        removeInstance,
        shutdown,
        response,
        exception
    }
}
