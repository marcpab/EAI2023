using EAI.LoggingV2;
using EAI.LoggingV2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Sql
{
    public class LogWriter : ILogWriter
    {
        public string ConnectionString { get; set; }

        public async Task WriteLogAsync(LogRecord record)
        {
            await using (var conn = await Connection.CreateAndOpenAsync(ConnectionString))
            {
                switch (record._logAction)
                {
                    case LogActionEnum.start:

                        await EaiDbCommands.AddProcess(conn,
                            record._processId,
                            record._stage,
                            record._processData._parentProcessId,
                            record._processData._parentStage,
                            record._processData._serviceName,
                            record._processData._initialMessageKey,
                            record._createdOnUTC
                            );

                        break;

                    case LogActionEnum.update:

                        await EaiDbCommands.UpdateProcess(conn,
                                record._processId,
                                record._stage,
                                record._processData._parentProcessId,
                                record._processData._parentStage,
                                record._processData._initialMessageKey,
                                record._createdOnUTC
                            );

                        break;
                    case LogActionEnum.logRecord:

                        break;
                    case LogActionEnum.leave:

                        await EaiDbCommands.SetProcessFinished(conn,
                                record._processId,
                                record._stage,
                                record._processData._status == StatusEnum.success,
                                record._processData._status == StatusEnum.failed,
                                record._createdOnUTC
                            );

                        break;
                }

                if (record._logData != null)
                {
                    var id_log = await EaiDbCommands.AddLog(conn,
                            record._processId,
                            record._stage,
                            record._logData._logLevel,
                            record._logData._messageKey,
                            record._logData._logText,
                            record._logData._message?._name,
                            record._logData._message?._type,
                            record._logData._message?._hash,
                            record._logData._message?._contentType,
                            record._logData._message?._content,
                            record._createdOnUTC
                        );

                    if (record._logData._exceptions != null)
                        foreach (var exceptionData in record._logData._exceptions)
                            await EaiDbCommands.AddException(conn,
                                    id_log,
                                    exceptionData._level,
                                    exceptionData._type,
                                    exceptionData._message,
                                    exceptionData._source,
                                    exceptionData._stackTrace,
                                    exceptionData._targetSite,
                                    exceptionData._hResult,
                                    record._createdOnUTC
                                );
                }
            }

        }
    }
}
