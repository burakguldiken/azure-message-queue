﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Base64ToString
{
    public class Encode
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
