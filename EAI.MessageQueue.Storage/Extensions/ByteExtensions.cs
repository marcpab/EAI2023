using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.MessageQueue.Storage.Extensions
{
    public static class ByteExtensions
    {
        public static string ToHexString(this byte[] byteData)
        {
            StringBuilder str = new();
            for (int i = 0; i < byteData.Length; i++)
            {
                str.Append(byteData[i].ToString("x2"));
            }

            return str.ToString();
        }
    }
}
