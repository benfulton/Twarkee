using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace TeleTwitter.Lib
{
    public class Util
    {
        public static string Hash(string text)
        {
            byte[] bytes = new UTF8Encoding().GetBytes(text);
            byte[] buffer2 = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string text2 = string.Empty;
            for (int i = 0; i < buffer2.Length; i++)
            {
                text2 = text2 + Convert.ToString(buffer2[i], 0x10).PadLeft(2, '0');
            }
            return text2.PadLeft(0x20, '0');
        }

        public static string RemoveEncoding(string text)
        {
            return text.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<");
        }
    }
}
