using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Oversite.PublicApi.Service
{


    public class AesEncryptionService
    {
        // Custom decryption logic (replace with actual encryption/decryption algorithm)

        private readonly string key = "1234567890123456"; // Same key as used in Angular

        public string Decrypt(string encryptedText)
        {
            byte[] iv = Encoding.UTF8.GetBytes(key); // The IV must be 16 bytes long (AES-CBC)
            byte[] buffer = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        public string Decode(string encodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(encodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }


        //private string Decode(string encryptedValue)
        //{
        //    var base64EncodedBytes = Convert.FromBase64String(encryptedValue);
        //    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //}


        public string HStr(string input)
        {
            string hiddenString = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                int charCode = (int)input[i];
                int hiddenCharCode = charCode + 1; // Shift the ASCII value by 1
                char hiddenChar = (char)hiddenCharCode;
                hiddenString += hiddenChar;
            }
            Console.WriteLine(hiddenString);

            hiddenString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(hiddenString));

            return hiddenString;
        }

        public string RStr(string hiddenString)
        {
            byte[] data = Convert.FromBase64String(hiddenString);
            string revealedStringH = System.Text.Encoding.UTF8.GetString(data);

            string revealedString = string.Empty;
            for (int i = 0; i < revealedStringH.Length; i++)
            {
                int hiddenCharCode = (int)revealedStringH[i];
                int revealedCharCode = hiddenCharCode - 1; // Reverse the ASCII value shift
                char revealedChar = (char)revealedCharCode;
                revealedString += revealedChar;
            }
            return revealedString;
        }


    }

}
