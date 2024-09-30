using System.Security.Cryptography;
using System.Text;

namespace CTMS.Core
{
    public class TpiAesUtil
    {
        //private static readonly string CIPHER_NAME = "AES/CBC/PKCS5PADDING";
        //private static readonly int CIPHER_KEY_LEN = 16;
        private string iv = "fedcba9876543210";
        public string key { get; set; }
        public string _encrypted { get; set; }
        public string _decrypted { get; set; }
        public TpiAesUtil(string accesKey)
        {
            key = accesKey;
        }
        public static TpiAesUtil getInstance(string accessKey)
        {
            return new TpiAesUtil(accessKey);
        }
        public string EncryptAes(string raw)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector(IV).
                // Same key must be used in encryption and decryption
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string 
                    // Print IV Vector
                    Console.WriteLine($"IV Vector: {(iv)}");
                    Console.WriteLine($"Key IN: {(key)}");
                    return Encrypt(raw, Encoding.UTF8.GetBytes(key),
                    Encoding.UTF8.GetBytes(iv));

                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return "Error";
        }
        public string DecryptAes(string raw)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector(IV).
                // Same key must be used in encryption and decryption
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string
                    // Print IV Vector
                    Console.WriteLine($"IV Vector: {(iv)}");

                    return
                   Decrypt(Convert.FromBase64String(raw.Split(':')[0].Trim().ToString()),
                   Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));

                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                throw;
            }
            return "Error";
        }
        static string Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.
            // Print IV Vector
            //Console.WriteLine($"IV Encrypt Vector:{ System.Text.Encoding.UTF8.GetString(IV)}");
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption
                    // and encrypts and decrypts data from any given stream.In this case, we will pass a memory stream
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
            return Convert.ToBase64String(encrypted) + ":" +
           Convert.ToBase64String(IV);
        }

        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Print IV Vector
            // Console.WriteLine($"IV Decrypt Vector: { System.Text.Encoding.UTF8.GetString(IV)} ");
            // Create AesManaged
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream
                    using (CryptoStream cs = new CryptoStream(ms,
                    decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream
                        using (StreamReader reader = new
                        StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }

    public class SHAUtil
    {
        //SHA-512 only
        public string hashCal(string type, string str)
        {
            try
            {
                return GenerateSHA512String(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return null;
        }
        private string GenerateSHA512String(string str)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
        private string GetStringFromHash(byte[] hash)
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
