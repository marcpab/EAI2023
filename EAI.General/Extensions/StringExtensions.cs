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

            if(long.TryParse(s, out long value))
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

            if (int.TryParse(s, out int value))
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

            if (DateTimeOffset.TryParseExact(s, format, null, DateTimeStyles.None, out DateTimeOffset value))
                return value;

            onParseFailed?.Invoke();
            return defaultValue;
        }

        public static byte[] CalculateSipHash(this string content, Encoding encoding = null, ulong k0 = 0, ulong k1 = 0)
        {
            if(encoding == null) 
                encoding = Encoding.UTF8;

            return BitConverter.GetBytes(SipHash.SipHash_2_4_UlongCast_ForcedInline(encoding.GetBytes(content), k0, k1));
        }

    }
}
