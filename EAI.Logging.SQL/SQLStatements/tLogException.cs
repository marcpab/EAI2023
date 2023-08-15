using EAI.Logging.Model;
using System.Data;
using System.Data.SqlClient;

namespace EAI.Logging.SQL
{
#pragma warning disable IDE1006 // Naming Styles
    internal class tLogException
#pragma warning restore IDE1006 // Naming Styles
    {
        public static
#if NETSTANDARD2_1
            async Task 
#else
            void
#endif
            AddDebugLogExceptionAsync(SqlConnectionAsync connection, LogException exceptionItem, long logId)
        {
            var cmd = new SqlCommand(@"dbo.up_AddLogException", connection);
#if NETSTANDARD2_1
            await
#else
            using (cmd)
#endif
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id_Log", SqlDbType.BigInt, 0, logId);
                cmd.AddParameter("@NestLevel", SqlDbType.Int, 200, exceptionItem.NestLevel);
                cmd.AddParameter("@Exception", SqlDbType.NVarChar, 200, exceptionItem.Exception ?? string.Empty);
                cmd.AddParameter("@Message", SqlDbType.NVarChar, 200, exceptionItem.Message ?? string.Empty);
                cmd.AddParameter("@Source", SqlDbType.NVarChar, 200, exceptionItem.Source ?? string.Empty);
                cmd.AddParameter("@HResult", SqlDbType.Int, 0, exceptionItem.HResult);
                cmd.AddParameter("@TargetSite", SqlDbType.NVarChar, 200, exceptionItem.TargetSite ?? string.Empty);
                cmd.AddParameter("@StackTrace", SqlDbType.NVarChar, 200, exceptionItem.StackTrace ?? string.Empty);

#if NETSTANDARD2_1
                await cmd.ExecuteNonQueryAsync();
#else
                cmd.ExecuteNonQuery();
#endif
            }
        }
    }
}
