using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.Model
{
    public class RfcFunctionMetadata : RfcContainerMetadata<RfcParameterMetadata>
    {
        public string _name;
        public RfcParameterMetadata[] _parameters;
    }
}
