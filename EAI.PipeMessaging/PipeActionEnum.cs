using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.PipeMessaging
{
    public enum PipeActionEnum
    {
        createInstance,
        request,
        removeInstance,
        shutdown,
        response,
        exception
    }
}
