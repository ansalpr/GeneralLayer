using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace GeneralLayer
{
    public class ManagedAesSample
    {
            
        public string EncryptData(string data, string key)
        {
            return EncryptAesManaged(data, key);
        }
        public string DecryptData(string data, string key)
        {
            return DecryptAesManaged(data, key);
        }

        public string EncryptPK(string pk,string key)
        {
            return EncryptPrimary(pk, key);
        }
        public string DecryptPK(string encPk, string key)
        {
            return DecryptPrimary(encPk, key);
        }
        public string generateKey()
        {
            string Key;
            try
            {
                System.Security.Cryptography.AesCryptoServiceProvider crypto = new System.Security.Cryptography.AesCryptoServiceProvider();
                crypto.KeySize = 128;
                crypto.BlockSize = 128;
                crypto.GenerateKey();
                byte[] keyGenerated = crypto.Key;
                Key = Convert.ToBase64String(keyGenerated);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Key;
        }
        static string EncryptAesManaged(string raw,string Key)
        {
            string encryptString = "";
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption 
                using (AesManaged aes = new AesManaged())
                {  
                    aes.Key =  Encoding.UTF8.GetBytes(Key);
                    aes.IV = new byte[16];
                    byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    encryptString= Convert.ToBase64String(encrypted);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return encryptString;
        }
        static string DecryptAesManaged(string raw, string Key)
        {
            string decrypted="";
            try
            {                
                using (AesManaged aes = new AesManaged())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = new byte[16];                    
                    byte[] temp_backToBytes = Convert.FromBase64String(raw);
                    decrypted = Decrypt(temp_backToBytes, aes.Key, aes.IV);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return decrypted;
        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }


        static string EncryptPrimary(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        static string DecryptPrimary(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}

