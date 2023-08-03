using EAI.SAPNco.IDOC.Model.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.SAPNco.IDOC.Metadata
{
    public static class EDI_DC40Extensions
    {
        public static string GetIdocType(this EDI_DC40 dc40)
            => $"{dc40.IDOCTYP}/{dc40.CIMTYP}/{dc40.DOCREL}/{dc40.MESTYP}";

        public static string GetIdocNumber(this EDI_DC40 dc40)
            => dc40.DOCNUM;
    }
}
