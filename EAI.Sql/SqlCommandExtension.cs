using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EAI.Sql
{
    public static class SqlCommandExtension
    {
        public static void AddParameter(this SqlCommand cmd, string name, SqlDbType type, int size, object? value)
        {
            switch (value)
            {
                case string stringValue:
                    value = stringValue.Replace("&#x1F;", string.Empty);

                    break;
                default:
                    break;
            }

            var param = new SqlParameter(name, type, size);

            param.Value = value ?? DBNull.Value;

            cmd.Parameters.Add(param);
        }

        public static SqlParameter AddParameterOutput(this SqlCommand cmd, string name, SqlDbType type, int size)
        {
            var param = new SqlParameter(name, type, size);

            param.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(param);

            return param;
        }
    }
}