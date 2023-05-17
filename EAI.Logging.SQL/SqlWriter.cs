using EAI.Logging;
using EAI.Logging.Model;
using EAI.Logging.SQL;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Roedl.Azure.Standard.Sql.Diagnostics
{
    public class SqlWriterId : ILogWriterId
    {
        public string Name => "SqlWriter";
    }

    public class SqlWriter : ILogWriter 
    {
        public ILogWriterId Id { get; set; } = new SqlWriterId();
        public string Connection { get; private set; }
        public string Interface { get; private set; }        
        public WriteAsyncResult Result { get; private set; }
        public string ResultMessage { get; private set; }

        public SqlWriter(string connection, string serviceName)
        {
            Connection = connection;
            Interface = serviceName;
        }

        public async Task WriteAsync(LogItem logEntry, CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = await GetConnectionAsync();
#if NETSTANDARD2_1
                await using (connection)
#else
                using (connection)
#endif
                {
                    long? messageId = null;
                    if (logEntry.LogMessage != null)
                        messageId =
#if NETSTANDARD2_1
                            await 
#endif
                            tMessage.AddMessageAsync(
                                connection, 
                                logEntry.LogMessage);

                    long logId =
#if NETSTANDARD2_1
                        await
#endif
                        tLog.AddDebugLogAsync(
                        connection,
                        logEntry,
                        messageId);

                    if (logEntry.Exceptions != null)
                    {
                        foreach (var logExceptionEntry in logEntry.Exceptions)
                        {
#if NETSTANDARD2_1
                            await 
#endif
                            tTaskException.AddDebugLogExceptionAsync(
                                connection,
                                logExceptionEntry,
                                logId
                            );
                        }
                    }
                }

                Result = WriteAsyncResult.Success;

                return;
            }
            catch (SqlException sex)
#if NETSTANDARD2_1
            when (!sex.IsFatal())
#endif
            {
                ResultMessage = sex.Message;

                // Azure related SQL throttling 
                // seems ex.Number is not comming in as expected, try to catch messages
                if (sex.Message.Contains("The client was unable to establish a connection because of an error during connection initialization process before login"))
                {
                    /*
                    * The client was unable to establish a connection because of an error during connection initialization 
                    * process before login. Possible causes include the following:  the client tried to connect to an 
                    * unsupported version of SQL Server; the server was too busy to accept new connections; or there was a 
                    * resource limitation (insufficient memory or maximum allowed connections) on the server. 
                    * (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)"}
                    */
                    Result = WriteAsyncResult.RetryRecommended;
                    return;
                }

                if (sex.Message.Contains("The client was unable to establish a connection because of an error during connection initialization process before login"))
                {
                        /*
                        * System.Data.Entity.Core.EntityException: The underlying provider failed on Open. ---> 
                        * System.InvalidOperationException: Timeout expired. The timeout period elapsed prior to obtaining a 
                        * connection from the pool. This may have occurred because all pooled connections were in use and max 
                        * pool size was reached
                        */
                    Result = WriteAsyncResult.RetryRecommended;
                    return;
                }
                    
                switch (sex.Number)
                {
                    case -2:
                    case -1:
                        // temporary errors like timeout, deadlock etc.
                    case 547:
                        // statement conflicted with the FOREIGN KEY constraint 
                        
                    case 1204:
                        // The instance of the SQL Server Database Engine cannot obtain a LOCK resource at this time.
                        // Rerun your statement when there are fewer active users. Ask the database administrator to check
                        // the lock and memory configuration for this instance, or to check for long-running transactions.
                    case 1205: 
                        // Transaction (Process ID) was deadlocked on resources with another process and has been chosen as
                        // the deadlock victim. Rerun the transaction
                    case 1206:
                        // The Microsoft Distributed Transaction Coordinator (MS DTC) has cancelled the distributed transaction.
                        Result = WriteAsyncResult.RetryRecommended;
                        break;
                    default:
                        Result = WriteAsyncResult.Failed;
                        throw;
                }
            }
            catch (AggregateException aex)
            {
                Result = WriteAsyncResult.Failed;
                ResultMessage = aex.Message;
                throw;
            }
            catch (Exception ex)
            {
                Result = WriteAsyncResult.Failed;
                ResultMessage = ex.Message;
                throw;
            }
        }

        private async Task<SqlConnectionAsync> GetConnectionAsync()
        {
            if (!Connection.EndsWith(";"))
                Connection += ";";

            if (Connection.Contains("Application Name"))
                return await SqlConnectionAsync.OpenAsync(Connection);

            return await SqlConnectionAsync.OpenAsync($"{Connection}Application Name={Id.Name} {Interface}");
        }
    }
}
