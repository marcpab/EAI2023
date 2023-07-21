using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public enum RfcServerStateEnum
    {
        Starting,
        Running,
        Broken,
        Stopping,
        Stopped,
        Invalid
    }
}
