using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.LoggingV2.Levels
{
    public class Info : ILogLevel
    {
        public string Name => nameof(Info);
    }
}
