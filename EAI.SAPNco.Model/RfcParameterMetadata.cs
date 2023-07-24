using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.Model
{
    public class RfcParameterMetadata : RfcElementMetadata
    {
        public RfcParameterDirection _direction;
        public string _defaultValue;
        public bool _optional;

        public RfcStructureMetadata _structureMetadata;
        public RfcTableMetadata _tableMetadata;

        public override string ToString()
            => $"PARAMETER: {_name}, {_direction}, {_rfcDataType}";

    }
}
