using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EAI.General.Extensions
{
    public static class StringExtensions
    {
        public static long? TryParseLong(this string s, long? defaultValue = null, Action onEmptyString = null, Action onParseFailed = null) 
        {  
            if(string.IsNullOrWhiteSpace(s))
            {
                onEmptyString?.Invoke();
                return defaultValue;
            }

            long value;
            if(long.TryParse(s, out value))
                return value;

            onParseFailed?.Invoke();
            return defaultValue;
        }

        public static int? TryParseInt(this string s, int? defaultValue = null, Action onEmptyString = null, Action onParseFailed = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                onEmptyString?.Invoke();
                return defaultValue;
            }

            int value;
            if (int.TryParse(s, out value))
                return value;

            onParseFailed?.Invoke();
            return defaultValue;
        }

        public static DateTimeOffset? TryParseDateTimeOffset(this string s, string format = null, DateTimeOffset? defaultValue = null, Action onEmptyString = null, Action onParseFailed = null)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                onEmptyString?.Invoke();
                return defaultValue;
            }

            DateTimeOffset value;
            if (DateTimeOffset.TryParseExact(s, format, null, DateTimeStyles.None, out value))
                return value;

            onParseFailed?.Invoke();
            return defaultValue;
        }
    }
}
