﻿using System;
using System.Data;
using System.Threading.Tasks;

namespace EAI.Sql
{
    internal class EaiDbCommands
    {
        public static async Task AddProcess(Connection conn, string id, string stage, string parentProcess, string parentStage, string service, string messageKey, DateTimeOffset createdOnUTC)
        {
            using(var cmd = conn.CreateCommand("dbo.up_AddProcess"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id",             SqlDbType.NVarChar, 50, id);
                cmd.AddParameter("@Stage",          SqlDbType.NVarChar, 20, stage ?? string.Empty);
                cmd.AddParameter("@Id_ParentProcess",
                                                    SqlDbType.NVarChar, 50, parentProcess);
                cmd.AddParameter("@ParentStage",    SqlDbType.NVarChar, 20, parentStage);
                cmd.AddParameter("@Service",        SqlDbType.NVarChar, 250, service);
                cmd.AddParameter("@MessageKey",     SqlDbType.NVarChar, 100, messageKey);
                cmd.AddParameter("@CreatedOnUTC",   SqlDbType.DateTimeOffset, 
                                                                        0, createdOnUTC);

                await Command.ExecuteSqlTaskAsync(cmd.ExecuteNonQueryAsync);
            }
        }

        public static async Task UpdateProcess(Connection conn, string id, string stage, string parentProcess, string parentStage, string messageKey, DateTimeOffset createdOnUTC)
        {
            using (var cmd = conn.CreateCommand("dbo.up_UpdateProcess"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id",             SqlDbType.NVarChar, 50, id);
                cmd.AddParameter("@Stage",          SqlDbType.NVarChar, 20, stage ?? string.Empty);
                cmd.AddParameter("@Id_ParentProcess", 
                                                    SqlDbType.NVarChar, 50, parentProcess);
                cmd.AddParameter("@ParentStage",    SqlDbType.NVarChar, 20, parentStage);
                cmd.AddParameter("@MessageKey",     SqlDbType.NVarChar, 100, messageKey);
                cmd.AddParameter("@CreatedOnUTC",   SqlDbType.DateTimeOffset, 
                                                                        0, createdOnUTC);

                await Command.ExecuteSqlTaskAsync(cmd.ExecuteNonQueryAsync);
            }
        }

        public static async Task SetProcessFinished(Connection conn, string id, string stage, bool isSuccess, bool isFailed, DateTimeOffset createdOnUTC)
        {
            using (var cmd = conn.CreateCommand("dbo.up_SetProcessFinished"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id",             SqlDbType.NVarChar, 50, id);
                cmd.AddParameter("@Stage",          SqlDbType.NVarChar, 20, stage ?? string.Empty);
                cmd.AddParameter("@isSuccess",      SqlDbType.Bit,      0, isSuccess);
                cmd.AddParameter("@isFailed",       SqlDbType.Bit,      0, isFailed);
                cmd.AddParameter("@FinishedOnUTC",  SqlDbType.DateTimeOffset, 
                                                                        0, createdOnUTC);

                await Command.ExecuteSqlTaskAsync(cmd.ExecuteNonQueryAsync);
            }
        }

        public static async Task<long> AddLog(Connection conn, string id_process, string stage, string logLevel, string messageKey, string logText, string? messageName, string? messageType, byte[]? messageHash, string? messageContentType, string? messageContent, DateTimeOffset createdOnUTC)
        {
            using (var cmd = conn.CreateCommand("dbo.up_AddLog"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id_Process",     SqlDbType.NVarChar, 50, id_process);
                cmd.AddParameter("@Stage",          SqlDbType.NVarChar, 20, stage ?? string.Empty);
                cmd.AddParameter("@LogLevel",       SqlDbType.NVarChar, 20, logLevel);
                cmd.AddParameter("@MessageKey",     SqlDbType.NVarChar, 100, messageKey);
                cmd.AddParameter("@LogText",        SqlDbType.NVarChar, 500, logText);
                cmd.AddParameter("@MessageName",    SqlDbType.NVarChar, 100, messageName);
                cmd.AddParameter("@MessageType",    SqlDbType.NVarChar, 250, messageType);
                cmd.AddParameter("@MessageHash",    SqlDbType.Binary,   8, messageHash);
                cmd.AddParameter("@MessageContentType", 
                                                    SqlDbType.NVarChar, 250, messageContentType);
                cmd.AddParameter("@MessageContent", SqlDbType.NVarChar, 10000, messageContent);
                cmd.AddParameter("@CreatedOnUTC",   SqlDbType.DateTimeOffset, 0, createdOnUTC);

                var id_log = cmd.AddParameterOutput("@Id_Log", SqlDbType.BigInt, 0);

                await Command.ExecuteSqlTaskAsync(cmd.ExecuteNonQueryAsync);

                return (long)id_log.Value;

            }
        }

        public static async Task AddException(Connection conn, long id_log, int level, string type, string message, string source, string stackTrace, string targetSite, int hResult, DateTimeOffset createdOnUTC)
        {
            using (var cmd = conn.CreateCommand("dbo.up_AddException"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.AddParameter("@Id_log",         SqlDbType.BigInt,   50, id_log);
                cmd.AddParameter("@Level",          SqlDbType.Int,      0, level);
                cmd.AddParameter("@Type",           SqlDbType.NVarChar, 250, type);
                cmd.AddParameter("@Message",        SqlDbType.NVarChar, 1000, message);
                cmd.AddParameter("@Source",         SqlDbType.NVarChar, 250, source);
                cmd.AddParameter("@StackTrace",     SqlDbType.NVarChar, 0, stackTrace);
                cmd.AddParameter("@TargetSite",     SqlDbType.NVarChar, 250, targetSite);
                cmd.AddParameter("@HResult",        SqlDbType.Int,      0, hResult);
                cmd.AddParameter("@CreatedOnUTC",   SqlDbType.DateTimeOffset, 
                                                                        0, createdOnUTC);

                await Command.ExecuteSqlTaskAsync(cmd.ExecuteNonQueryAsync);
            }
        }


    }
}