using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.LoggingV2.Model
{
    public class ExceptionData
    {
        public int _level;
        public string _type;
        public string _message;
        public string _source;
        public string _targetSite;
        public string _stackTrace;
        public int _hResult;
    }
}
