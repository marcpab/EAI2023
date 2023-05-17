using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EAI.Logging.Model
{
    public interface ILogStage
    {
        LogStage Stage { get; }
        String Description { get; }
        string ToString();
    }
}
