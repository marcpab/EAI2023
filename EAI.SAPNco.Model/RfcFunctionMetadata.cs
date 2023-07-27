using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.Model
{
    public class RfcFunctionMetadata
    {
        public string _name;
        public RfcParameterMetadata[] _parameters;
        public string _description;

        public override string ToString()
            => $"FUNCTION: {_name}";
    }
}
