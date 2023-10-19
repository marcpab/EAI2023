using EAI.SAPNco.Utils.Model;
using System.Collections.Generic;
using System.Linq;

namespace EAI.SAPNco.Utils.ReadTable
{
    public class SapReadTableResult
    {
        private RFC_READ_TABLE_Response _RFC_READ_TABLE_Response;

        public SapReadTableResult(RFC_READ_TABLE_Response rFC_READ_TABLE_Response)
        {
            _RFC_READ_TABLE_Response = rFC_READ_TABLE_Response;
        }

        public IEnumerable<SapReadTableField> Fields { get => _RFC_READ_TABLE_Response.FIELDS.Select(f => new SapReadTableField(f)); }


        public IEnumerable<SapReadTableRow> Rows { get => _RFC_READ_TABLE_Response.DATA.Select((x, i) => new SapReadTableRow(x, i, _RFC_READ_TABLE_Response.FIELDS)); }
    }
}