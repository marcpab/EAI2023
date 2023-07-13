using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Sql
{
    public class Command
    {
        private static int[] _retryErrors = new int[] { 
                    (int)SqlErrorEnum.NetworkError,
                    (int)SqlErrorEnum.LockResources,
                    (int)SqlErrorEnum.DTC,
                    (int)SqlErrorEnum.Deadlock 
                };

        public static async Task ExecuteSqlTaskAsync(Func<Task> sqlTaskAsync, int retry = 3, int retryWaitMilliseconds = 250, int retryTimeout = 1)
            => await ExecuteSqlTaskAsync<object>(
                async () => { 
                    await sqlTaskAsync(); 

                    return null; 
                }, 
                retry, 
                retryWaitMilliseconds, 
                retryTimeout);

        public static async Task<T> ExecuteSqlTaskAsync<T>(Func<Task<T>> sqlTaskAsync, int retry = 3, int retryWaitMilliseconds = 250, int retryTimeout = 1)
        {
            while(true)
                try
                {
                    return await sqlTaskAsync();
                }
                catch(SqlException ex) when(_retryErrors.Contains(ex.Number))
                {
                    retry--;
                    if (retry == 0)
                        throw;

                    await Task.Delay(retryWaitMilliseconds);

                    retryWaitMilliseconds += retryWaitMilliseconds;
                }
                catch (SqlException ex) when (ex.Number == (int)SqlErrorEnum.Timeout)
                {
                    retryTimeout--;
                    if (retryTimeout == 0)
                        throw;
                }
        }
    }
}
