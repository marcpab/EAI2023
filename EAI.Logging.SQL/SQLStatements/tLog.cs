using System;
using System.Data.SqlClient;
using System.Data;
using EAI.Logging.Model;

namespace EAI.Logging.SQL
{
    internal class tLog
    {
        public static
#if NETSTANDARD2_1
            async Task<long> 
#else
            long
#endif
            AddDebugLogAsync(SqlConnectionAsync connection, LogItem item, long? idMessage)
        {
            var cmd = new SqlCommand(@"dbo.up_AddLog", connection);
#if NETSTANDARD2_1
            await using (cmd.ConfigureAwait(false))
#else
            using (cmd)
#endif
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id_Level", SqlDbType.Int, 0, (int)item.Level);
                cmd.AddParameter("@Id_Stage", SqlDbType.Int, 0, item.StageId);
                cmd.AddParameter("@Service", SqlDbType.NVarChar, 200, item.Service ?? string.Empty);
                cmd.AddParameter("@Transaction", SqlDbType.NVarChar, 40, item.Transaction ?? string.Empty);
                cmd.AddParameter("@TransactionHash", SqlDbType.Int, 0, item.Transaction);
                cmd.AddParameter("@ChildTransaction", SqlDbType.NVarChar, 40, item.ChildTransaction ?? string.Empty);
                cmd.AddParameter("@TransactionKey", SqlDbType.NVarChar, 200, item.TransactionKey ?? string.Empty);
                cmd.AddParameter("@Description", SqlDbType.NVarChar, int.MaxValue, item.Description ?? string.Empty);
                cmd.AddParameter("@Operation", SqlDbType.NVarChar, 200, item.LogMessage?.Operation ?? string.Empty);
                cmd.AddParameter("@Id_Message", SqlDbType.BigInt, 0, (object)idMessage ?? DBNull.Value);
                cmd.AddParameter("@CreatedOnUTC", SqlDbType.DateTime, 0, (object)item.CreatedOnUTC ?? DBNull.Value);

                var idDebugLog = cmd.AddOutputParameter("@id_DebugLog", SqlDbType.BigInt, 0);


#if NETSTANDARD2_1
                return await cmd
                    .ExecuteNonQueryAsync()
                    .ContinueWith(task => (long)(id.Value ?? (long)0))
                    .ConfigureAwait(false);
#else
                var res = cmd.ExecuteNonQuery();
                return (long)(cmd.Parameters["@id"].Value ?? 0L);
#endif
            }
        }
    }
}
