using System;
using System.Globalization;

namespace EAI.General.SAPECC.Extensions
{
    public static class StringExtension
    {
        public static string RemoveLeadingZeros(this string original)
        {
            if (string.IsNullOrWhiteSpace(original))
                return original;

            var trimmed = original.TrimStart(new char[] { '0' });

            if (trimmed.Length == 0)
                return "0";

            return trimmed;
        }

        public static DateTime? ParseSAPDate(this string sap, bool whenEmptySetNow = false)
        {
            if (string.IsNullOrWhiteSpace(sap) || sap == "00000000")
                if (whenEmptySetNow)
                    return DateTime.Now.Date;
                else
                    return null;

            return DateTime.ParseExact(sap, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static DateTime? ParseSAPTime(this string sap, bool whenEmptySetNow = false)
        {
            if (string.IsNullOrWhiteSpace(sap) || sap == "000000")
                if (whenEmptySetNow)
                    return DateTime.Now;
                else
                    return null;

            return DateTime.ParseExact(sap, "HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static DateTime? ParseSAPDateTime(this string sap, bool whenEmptySetNow = false)
        {
            if (string.IsNullOrWhiteSpace(sap) || sap == "000000000000")
                if (whenEmptySetNow)
                    return DateTime.Now;
                else
                    return null;

            return DateTime.ParseExact(sap, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}
