using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Assignment3
{
    class SHA
    {

        public static string GenerateSHA512String(string input)
        {
            //INITIALIZE AN OBJECT SHA512 THROUGH CREATE METHOD 
            SHA512 sha512 = SHA512Managed.Create();
            // CREATE ARRAY OF BYTES FROM INPUT
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            //CONVERTS THE ARRAYOF BITES INTO ARRAY OF HASHED BYTES
            byte[] hash = sha512.ComputeHash(bytes);
            //CONVERTS BYTES TO STRING
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
