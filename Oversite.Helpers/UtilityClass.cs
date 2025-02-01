using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Helpers
{
    public class UtilityClass
    {
        public string generateOTP()
        {
            int lenthofpass = 6;
            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9";
            char[] sep = new[] { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i <= lenthofpass - 1; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }
    }
}
