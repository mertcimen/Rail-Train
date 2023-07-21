using System;
using System.Security.Cryptography;
using System.Text;

namespace FunGames.Tools.Core
{
    public static class FGAPIHelpers
    {
        private static readonly char[] Array1 =
        {
            '\u0074', '\u0061', '\u0070', '\u006E', '\u0061', '\u0074', '\u0069', '\u006F', '\u006E', '\u002D',
            '\u0073', '\u0065', '\u0063', '\u0072', '\u0065', '\u0074'
        };

        private static string _secret = new string(Array1);

        internal static string CreateToken(string message)
        {
            _secret = _secret ?? "";

            var encoding = new ASCIIEncoding();
            var keyByte = encoding.GetBytes(_secret);
            var messageBytes = encoding.GetBytes(message);

            string dest;

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);
                dest = Convert.ToBase64String(hashmessage);
                dest = dest.Remove(dest.Length - 1);
                return dest;
            }
        }

        internal static string GetBitString()
        {
            if (!String.IsNullOrEmpty(FGMainSettings.settings.ApiKey)) return FGMainSettings.settings.ApiKey;

            var md5Hash = MD5.Create();
            var result = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(_secret));
            var bitString = BitConverter.ToString(result).Replace("-","").ToLower();
            
            return bitString;
        }
    }
}