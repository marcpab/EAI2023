using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EAI.Sql
{
    public class Connection : IAsyncDisposable, IDisposable
    {
        private SqlConnection? _connection;

        public static async Task<Connection> CreateAndOpenAsync(string connectionString, int retry = 3, int retryWaitMilliseconds = 250)
        {
            var conn = new Connection();
            await conn.OpenAsync(connectionString, retry, retryWaitMilliseconds);

            return conn;
        }

        public static implicit operator SqlConnection(Connection connection)
            => connection._connection;

        public void Dispose()
        {
            var conn = _connection;
            _connection = null;

            if (conn != null)
            {
                if(conn.State == ConnectionState.Open) 
                    conn.Close();

                conn.Dispose();
            }
        }

        public async ValueTask DisposeAsync()
        {
            var conn = _connection;
            _connection = null;

            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    await conn.CloseAsync();

                await conn.DisposeAsync();
            }
        }

        public async Task OpenAsync(string connectionString, int retry = 3, int retryWaitMilliseconds = 250)
        {
            while(true)
            {
                retry--;

                _connection = new SqlConnection(connectionString);

                try
                {
                    await _connection.OpenAsync();

                    return;
                }
                catch (SqlException)
                {
                    if (retry == 0)
                        throw;

                    await Task.Delay(retryWaitMilliseconds);
                    retryWaitMilliseconds += retryWaitMilliseconds;
                }
            }
        }

        public SqlCommand CreateCommand(string commandText)
            =>  new SqlCommand(commandText, _connection);
    }
}
