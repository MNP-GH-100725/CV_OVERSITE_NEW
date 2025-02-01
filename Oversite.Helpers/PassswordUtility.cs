using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CVHelpers
{
    public class PassswordUtility
    {
        public string Encrypt(string text)
        {
            byte[] bytes1 = Encoding.UTF8.GetBytes("8080808080808080");
            byte[] bytes2 = Encoding.UTF8.GetBytes("8080808080808080");
            string encryptedText = Convert.ToBase64String(EncryptStringToBytes(text, bytes1, bytes2));
            return encryptedText;
        }

        private const string _securityKey = "raju";

        public string DBDecrypt(string EncryptedText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(EncryptedText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));

            //De-allocatinng the memory after doing the Job.

            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            //Assigning the Security key to the TripleDES Service Provider.

            objTripleDESCryptoService.Key = securityKeyArray;

            //Mode of the Crypto service is Electronic Code Book.

            objTripleDESCryptoService.Mode = CipherMode.ECB;

            //Padding Mode is PKCS7 if there is any extra byte is added.

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

            //Transform the bytes array to resultArray

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Releasing the Memory Occupied by TripleDES Service Provider for Decryption.          

            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.

            return UTF8Encoding.UTF8.GetString(resultArray);

        }




        public string Decrypt(string text)
        {
            byte[] bytes1 = Encoding.UTF8.GetBytes("8080808080808080");
            byte[] bytes2 = Encoding.UTF8.GetBytes("8080808080808080");
            string decryptedText = DecryptStringFromBytes(Convert.FromBase64String(text), bytes1, bytes2);
            return decryptedText;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;
                rijndaelManaged.Key = key;
                rijndaelManaged.IV = iv;
                ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            streamWriter.Write(plainText);
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key,
                                             rijAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                           decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
        public bool CheckRepeatingSequence(string pswd)
        {
            try
            {
                bool flag = true;
                int len = pswd.Length;
                char first, second;

                for (int i = 0; i < len - 1; ++i)
                {
                    if (Char.IsDigit(pswd[i]))
                    {
                        first = pswd[i];
                        if (Char.IsDigit(pswd[i + 1]))
                        {
                            second = pswd[i + 1];
                            if (second == first + 1)
                                flag = false;
                            else if (second == first - 1)
                                flag = false;
                            else if (second == first)
                                flag = false;

                        }
                    }
                }



                return flag;
            }
            catch
            {
                return false;
            }
        }
    }
}
