using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EAI.OnPrem.SAPNcoService.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToSapDate(this DateTimeOffset? value)
        {
            if (value == null) 
                return null;

            return value.Value.ToString("yyyyMMdd");
        }
    }
}
