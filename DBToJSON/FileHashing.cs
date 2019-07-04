using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DBToJSON
{
    //http://gigi.nullneuron.net/gigilabs/computing-file-hashes-cs/
    public static class FileHashing
    {
        public static string MakeHash(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                using (HashAlgorithm hashAlgorithm = MD5.Create())
                {
                    byte[] hash = hashAlgorithm.ComputeHash(fs);
                    return ToHexString(hash);
                }
            }
        }

        private static string ToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}
