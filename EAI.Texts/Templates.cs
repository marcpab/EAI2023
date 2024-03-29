﻿using System;
using System.Linq;
using System.Net;
using System.Web;

namespace EAI.Texts
{
    public static partial class Templates
    {
        public static Func<Exception, string> ExceptionILogger = (ex) =>
        {
            if (ex is null)
                return $"Exception object is {EAI.Texts.Properties.NULL}";

            return $"{ex.Message} {ex.InnerException?.Message} stack: {ex.StackTrace}";
        };

        public static Func<HttpStatusCode, string> Leave = (statusCode) =>
        {
            var statusCodeText = $"{statusCode}";
            if (statusCode.GetHashCode().ToString() == statusCodeText)
                statusCodeText = "Custom";

            return $"{LogMessages.Leave} http code {statusCode.GetHashCode()} ({statusCodeText})";
        };

        public static Func<int, string> LeaveInt = (statusCode) =>
        {
            return Leave((HttpStatusCode)statusCode);
        };

        public static Func<string, string> EscapeOdataQuery = (string input) =>
        {
            var output = SanitizeProblematicWhitespaces(input);

            if (string.IsNullOrWhiteSpace(output))
                return output;

            output = output.Replace("'", "''");

            output = HttpUtility.UrlEncode(output);

            return output;
        };

        public static Func<string, string> SanitizeProblematicWhitespaces = (string input) =>
        {
            var output = input;

            // chaining is more memory efficient than looping an array
            return output?
                .Replace("\u00A0", " ")
                .Replace("\u2007", " ")
                .Replace("\u202F", " ")
                .Replace("\u200B", "");
        };

        public static Func<string, string> TrimAll = (string input) =>
        {
            if (string.IsNullOrWhiteSpace(input))
                return input?.Trim();

            return String.Concat(input.Where(c => !Char.IsControl(c)));
        };

        public static bool IsControl(char c)
        {
            // because 'c' can never be -1.
            // x80 = 128
            // & bitwise and
            // ~ bitwise complementary (inverting bits)
            return (((uint)c + 1) & ~0x80u) <= 0x20u;
        }
    }
}
