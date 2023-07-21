using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.NetFramework.SAPNco
{
    public enum RfcServerState
    {
        Starting,
        Running,
        Broken,
        Stopping,
        Stopped,
        Invalid
    }
}
