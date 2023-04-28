using System.Security.Cryptography;
using System.Text;
using BL.Configuration;
using FCBankBasicHelper.Attributes;

namespace BL.Encrypt
{
    public class Encryption : IEncryption
    {
        private readonly IConfigurationKey configuration;
        public Encryption(IConfigurationKey configuration)
        {
            this.configuration = configuration;
        }
        public void EncryptData(object data)
        {
            if (data == null) return;
            Type type = data.GetType();
            foreach (var property in type.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(PropertyAttribute), true);
                if (attributes.Length > 0)
                {
                    var value = property.GetValue(data).ToString();
                    if (value != null)
                    {
                        property.SetValue(data, Encrypt(value));
                    }
                }
            }
        }
        public void DecryptData(object data)
        {
            if (data == null) return;
            Type type = data.GetType();
            foreach (var property in type.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(PropertyAttribute), true);
                if (attributes.Length > 0)
                {
                    var value = property.GetValue(data).ToString();
                    if (value != null)
                    {
                        property.SetValue(data, Decrypt(value));
                    }
                }
            }
        }
        public string Encrypt(string data)
        {
            string key = configuration.EncryptKey;
            string bytekey = configuration.EncrpytByte;
            byte[] initializationVector = Encoding.ASCII.GetBytes(bytekey);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = initializationVector;
                var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream
                        (memoryStream as Stream, symmetricEncryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
                        {
                            streamWriter.Write(data);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }
        public string Decrypt(string cipherText)
        {
            string key = configuration.EncryptKey;
            string bytekey = configuration.EncrpytByte;
            byte[] initializationVector = Encoding.ASCII.GetBytes(bytekey);
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream
                        (memoryStream as Stream,
                        decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream as Stream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}