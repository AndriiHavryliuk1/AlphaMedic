using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Rest.Helpers
{
    public static class HashHelper
    {
        private static SHA256 hash = SHA256.Create();

        public static string sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();


            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));

            return Sb.ToString();
        }
    }
}