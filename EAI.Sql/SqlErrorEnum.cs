using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.Sql
{ 
    public enum SqlErrorEnum
    { 
        Timeout = -2,
        NetworkError = 11,
        LockResources = 1204,
        Deadlock = 1205,
        DTC = 1204,
    }
}
