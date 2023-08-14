using System;
using System.Data.SqlClient;
using System.Data;
using EAI.Logging.Model;
using System.Text;
using System.Security.Cryptography;

namespace EAI.Logging.SQL
{
    internal class tLog
    {
        static Int32 sha256(string content)
        {
            var crypt = new SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(content));
            
            return BitConverter.ToInt32(crypto, 0);
        }

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
                cmd.AddParameter("@Service", SqlDbType.NVarChar, 80, item.Service ?? string.Empty);
                cmd.AddParameter("@ParentProcessId", SqlDbType.NVarChar, 50, item.Transaction ?? string.Empty);
                cmd.AddParameter("@ParentProcessHash", SqlDbType.Int, 0, sha256(item.Transaction));
                cmd.AddParameter("@ProcessId", SqlDbType.NVarChar, 50, item.ChildTransaction ?? string.Empty);
                cmd.AddParameter("@MessageId", SqlDbType.NVarChar, 80, item.TransactionKey ?? string.Empty);
                cmd.AddParameter("@Text", SqlDbType.NVarChar, int.MaxValue, item.Description ?? string.Empty);
                cmd.AddParameter("@MessageName", SqlDbType.NVarChar, 80, item.LogMessage?.Operation ?? string.Empty);
                cmd.AddParameter("@Id_Message", SqlDbType.BigInt, 0, (object)idMessage ?? DBNull.Value);
                cmd.AddParameter("@CreatedOnUTC", SqlDbType.DateTime, 0, (object)item.CreatedOnUTC ?? DBNull.Value);

                var idDebugLog = cmd.AddOutputParameter("@Id_Log", SqlDbType.BigInt, 0);


#if NETSTANDARD2_1
                return await cmd
                    .ExecuteNonQueryAsync()
                    .ContinueWith(task => (long)(id.Value ?? (long)0))
                    .ConfigureAwait(false);
#else
                var res = cmd.ExecuteNonQuery();
                return (long)(cmd.Parameters["@Id_Log"].Value ?? 0L);
#endif
            }
        }
    }
}
