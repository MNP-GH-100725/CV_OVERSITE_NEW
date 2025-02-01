using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Maibro.Helper
{
    public class EncryptDecryptUtil
    {
        private string _securityKey = "raju";
        public byte[] getEdata(string userName, string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(userName + _securityKey + password));
            return hashedDataBytes;
        }

        public string getStringEncryptdata(string userName, string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(userName + _securityKey + password));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashedDataBytes.Length; i++)
            {
                sb.Append(hashedDataBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }


        public string Decrypt(string EncryptedText)
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
        public string Decrypt2(string EncryptedText)
        {
            string key = "7x!A%D*G-KaPdSgV";
            byte[] toEncryptArray = Convert.FromBase64String(EncryptedText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

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

        public string Encrypt(string Text)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(Text);

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
            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            //Transform the bytes array to resultArray

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

            //Releasing the Memory Occupied by TripleDES Service Provider for Encryption.

            objTripleDESCryptoService.Clear();

            //Convert and return the encrypted data/byte into string format.

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string DecryptProperty(string EncryptedText)
        {

            string ret = string.Empty;
            string key = "7x!A%D*G-KaPdSgV";
            var keybytes = Encoding.UTF8.GetBytes(key);
            var iv = Encoding.UTF8.GetBytes(key);
            var encrypted = Convert.FromBase64String(EncryptedText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return decriptedFromJavascript;

        }
        public string ToMD5(string input)
        {
            // Use input string to calculate MD5 hash
            StringBuilder sb1 = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                for (int i = 0; i < hashValue.Length; i++)
                {
                    sb1.Append(hashValue[i].ToString("X2"));
                }
            }


            //// Use input string to calculate MD5 hash
            //using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            //{
            //	byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            //	byte[] hashBytes = md5.ComputeHash(inputBytes);

            //	// Convert the byte array to hexadecimal string
            //	StringBuilder sb = new StringBuilder();
            //	for (int i = 0; i < hashBytes.Length; i++)
            //	{
            //		sb.Append(hashBytes[i].ToString("X2"));
            //	}
            return sb1.ToString().ToLower();
        }

        public string EncryptProperty(string EncryptedText)
        {
            string ret = string.Empty;
            string key = "7x!A%D*G-KaPdSgV";
            var keybytes = Encoding.UTF8.GetBytes(key);
            var iv = Encoding.UTF8.GetBytes(key);
            var decriptedFromJavascript = EncryptStringToBytes(EncryptedText, keybytes, iv);
            return ret = Convert.ToBase64String(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }
        public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
    }
}
