using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace COURSE_CODING.Common
{
    public class HashMD5
    {
        public static string HashStringMD5(String text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {            
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}