using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.Model
{
    public class RfcContainerMetadata<T> 
        where T : RfcElementMetadata
    {
        public string _name;

        public int _elementCount;
        public T[] _rfcElements;
    }
}
