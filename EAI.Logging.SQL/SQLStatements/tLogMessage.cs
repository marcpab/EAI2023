using EAI.Logging.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

namespace EAI.Logging.SQL
{
    internal class tLogMessage
    {
        public static
#if NETSTANDARD2_1
        async Task<long> 
#else
        long
#endif
        AddMessageAsync(SqlConnectionAsync connection, LogMessage message)
        {
            var cmd = new SqlCommand(@"dbo.up_AddLogMessage", connection);
#if NETSTANDARD2_1
            await
#else
            using (cmd)
#endif
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@messageType", SqlDbType.NVarChar, 5, message.MsgType);
                cmd.AddParameter("@messageContent", SqlDbType.NVarChar, int.MaxValue, message.Content);
                var id_Message = cmd.AddOutputParameter("@id_Message", SqlDbType.BigInt, 0);
                                    
#if NETSTANDARD2_1
                return await cmd
                    .ExecuteNonQueryAsync()
                    .ContinueWith(task => (long)(id_Message.Value ?? 0));
#else
                var res = cmd.ExecuteNonQuery();
                return (long)(cmd.Parameters["@id_Message"].Value ?? 0L);
#endif
            }
        }
    }
}
