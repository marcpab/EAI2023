using EAI.Logging.SQL;
using System;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Security;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace EAI.Logging.SQL
{
    public class SqlConnectionAsync : IDisposable
#if NETSTANDARD2_1
        , IAsyncDisposable
#endif
    {
        
        public SqlConnection Connection { get; }

        private SqlConnectionAsync(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public static async Task<SqlConnectionAsync> OpenAsync(string connectionString)
        {
            var con = new SqlConnectionAsync(connectionString);

            try
            {
                await con.OpenAsync();

                return con;
            }
            catch
            {
#if NETSTANDARD2_1
                await con.DisposeAsync();
#else
                con.Dispose();
#endif
                throw;
            }
        }

        private async Task OpenAsync() => 
            await Connection.OpenAsync();

        public static implicit operator SqlConnection(SqlConnectionAsync connection) =>
            connection.Connection;

        public void Dispose()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();

            Connection.Dispose();
        }

#if NETSTANDARD2_1
        public async ValueTask DisposeAsync()
        {
            if (Connection.State == ConnectionState.Open)
                await Connection.CloseAsync();

            await SqlConnection.DisposeAsync();
        }
#endif

        public SqlCommand CreateCommand(string command)
        {
            return new SqlCommand() 
            { 
                Connection = Connection, 
                CommandText = command
            };
        }

    }
}

