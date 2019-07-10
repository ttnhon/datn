using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COURSE_CODING
{
    public class OTP
    {
        public static string GenerateStringCodeOTP()
        {
            string numbers = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random objrandom = new Random();
            string passwordString = "";
            string strrandom = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                passwordString = numbers.ToCharArray()[temp].ToString();
                strrandom += passwordString;
            }
            return strrandom;
        }
    }
}