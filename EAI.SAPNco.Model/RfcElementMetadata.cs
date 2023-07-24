using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.Model
{
    public class RfcElementMetadata
    {
        public string _name;
        public string _documentation;
        public RfcDataType _rfcDataType;
        public string _abapDataType;
        public int _nucLength;
        public int _ucLength;
        public int _decimals;
    }
}
