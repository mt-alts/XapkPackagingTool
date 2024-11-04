/* 
   Copyright (c) 2024 Metin Altıkardeş
   Licensed under the MIT License. See the LICENSE.
*/

using System.Security.Cryptography;
using System.Text;

namespace XapkPackagingTool.Utility.Hashing
{
    internal static class ByteArrayHashCompute
    {
        public static string ComputeSha256Hash(byte[] data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(data);
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
