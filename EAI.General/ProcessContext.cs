﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EAI.General
{
    public class ProcessContext
    {
        private static AsyncLocal<ProcessContext> _executionContext = new AsyncLocal<ProcessContext>();

        public string ParentStage { get; set; }
        public string ParentProcessId { get; set; }

        public string Stage { get; set; }
        public string ProcessId { get; set; }
        public string ServiceName { get; set; }

        public static ProcessContext Create(string processId, string stage, string serviceName)
        {
            var parentContext = _executionContext.Value;
            
            var context = new ProcessContext();

            context.ParentStage = parentContext?.Stage;
            context.ParentProcessId = parentContext?.ProcessId;

            context.Stage = stage ?? parentContext?.Stage;
            context.ServiceName = serviceName ?? parentContext?.ServiceName;
            context.ProcessId = processId ?? Guid.NewGuid().ToString();

            _executionContext.Value = context;

            return context;
        }

        public static ProcessContext GetCurrent()
            => _executionContext.Value;

        public static void Restore(ProcessContext context) 
            => _executionContext.Value = context;

        public static void SetParentContext(ProcessContext parentProcessContext)
        {
            var context = GetCurrent();
            if (context != null)
            {
                context.ParentStage = parentProcessContext?.Stage;
                context.ParentProcessId = parentProcessContext?.ProcessId;
            }
        }
    }
}