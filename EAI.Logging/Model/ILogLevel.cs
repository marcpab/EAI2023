using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EAI.Logging.Model
{
    public interface ILogLevel
    {
        LogLevel Level { get; }
        string ToString();
    }
}
