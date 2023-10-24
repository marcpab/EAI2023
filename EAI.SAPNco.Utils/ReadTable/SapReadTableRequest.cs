using EAI.Abstraction.SAPNcoService;
using EAI.General.Extensions;
using EAI.SAPNco.Utils.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAI.SAPNco.Utils.ReadTable
{
    public class SapReadTableRequest
    {
        private string _tableName;
        private List<string> _fields;
        private string _where;
        private int _rowCount;
        private int _skipRows;

        public string TableName { get => _tableName; set => _tableName = value; }

        public List<string> Fields { get => _fields; set => _fields = value; }

        public string Where { get => _where; set => _where = value; }

        public int RowCount { get => _rowCount; set => _rowCount = value; }

        public int SkipRows { get => _skipRows; set => _skipRows = value; }


        public async Task<SapReadTableResult> ExecuteAsync(IRfcGatewayService rfcGatewayService, string sapSystemName)
        {
            var rfcReadTableRequest = new RFC_READ_TABLE_Message()
            {
                RFC_READ_TABLE = new RFC_READ_TABLE()
                {
                    DELIMITER = "|",
                    QUERY_TABLE = _tableName,
                    ROWCOUNT = _rowCount,
                    ROWSKIPS = _skipRows,
                    FIELDS = _fields.Select(f => new RFC_DB_FLD() { FIELDNAME = f }).ToList(),
                    GET_SORTED = "",
                    USE_ET_DATA_4_RETURN = "",
                    OPTIONS = _where
                                .Chunkify(72)
                                .Select(c => new RFC_DB_OPT() { TEXT = c })
                                .ToList()
                }
            };

            var rfcReadTableResponse = await rfcGatewayService.CallRfcAsync(sapSystemName, rfcReadTableRequest, false);

            return new SapReadTableResult(rfcReadTableResponse.RFC_READ_TABLE_Response);
        }
    }
}
