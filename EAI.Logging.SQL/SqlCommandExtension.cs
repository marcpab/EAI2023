using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Text;
using System.Xml;

namespace EAI.Logging.SQL
{
    internal static class SqlCommandExtension
    {
        public static readonly List<string> InvalidXml10Characters = 
            new List<string>()
            {
                "&#x1F;",
            };

        public const int MAX_PAGE_VAR_SIZE = 4000;

        public static void AddParameter(this SqlCommand command, string name, SqlDbType sqlDbType, int size, object value)
        {
            switch (sqlDbType)
            {
                case SqlDbType.Xml:
                    var valueString = value as string;

                    if (!string.IsNullOrEmpty(valueString))
                    {
                        var sb = new StringBuilder(valueString);
                        foreach (var invalid in InvalidXml10Characters)
                        {
                            sb = sb.Replace(invalid, string.Empty);
                        }

                        value = new SqlXml(new XmlTextReader(sb.ToString(), XmlNodeType.Document, null));
                        size = int.MaxValue;
                    }
                    else
                    {
                        value = null;
                    }
                    break;
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    if (size < MAX_PAGE_VAR_SIZE)
                        size = MAX_PAGE_VAR_SIZE;
                    break;
                default:
                    break;
            }

            var parameter = new SqlParameter(name, sqlDbType, size);
            parameter.Value = value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }

        public static SqlParameter AddOutputParameter(this SqlCommand command, string name, SqlDbType sqlDbType, int size)
        {
            if (size < MAX_PAGE_VAR_SIZE)
                switch (sqlDbType)
                {
                    case SqlDbType.NVarChar:
                    case SqlDbType.VarChar:
                        size = MAX_PAGE_VAR_SIZE;
                        break;
                    default:
                        break;
                }

            var parameter = new SqlParameter(name, sqlDbType, size);
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);

            return parameter;
        }
    }
}
