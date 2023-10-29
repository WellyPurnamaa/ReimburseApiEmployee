using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApi.Helpers
{
    public class DataEncription
    {
        public static string ComputeHash(string plainText,
                                    string hashAlgorithm)
        {
            if (hashAlgorithm == null)
                hashAlgorithm = "";
            if (hashAlgorithm.ToUpper() == "MD5")
            {
                // 1, calculate MD5 hash from input
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(plainText);
                byte[] hashmd5 = md5.ComputeHash(inputBytes);

                // 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashmd5.Length; i++)
                {
                    sb.Append(hashmd5[i].ToString("x2"));
                }
                return sb.ToString();
            }
            else if (hashAlgorithm.ToUpper() == "SHA1")
            {
                // 1, calculate MD5 hash from input
                SHA1Managed sha1 = new SHA1Managed();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(plainText);
                byte[] hashmd5 = sha1.ComputeHash(inputBytes);

                // 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashmd5.Length; i++)
                {
                    sb.Append(hashmd5[i].ToString("x2"));
                }
                return sb.ToString();
            }
            return "";
        }
    }
}
