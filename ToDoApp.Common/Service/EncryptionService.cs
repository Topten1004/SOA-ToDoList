using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToDoApp.Common.Service
{
    public class EncryptionService
    {
        private readonly string _saltPacket;
        private string _hashType = "MD5";
        private string _algType = "TripleDES";
        private readonly string _encryptorKey;
        private readonly string _ivString;

        public EncryptionService(string saltPacket, string key, string ivString)
        {
            if(string.IsNullOrEmpty(saltPacket) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(ivString))
                throw new ArgumentException("EncryptionService Constructor parameters not nullable.");

            _saltPacket = saltPacket;
            _encryptorKey = key;
            _ivString = ivString;
        }

        public string Decrypt(string hash)
        {
            hash = Encoding.ASCII.GetString(Convert.FromBase64String(hash));
            TripleDESCryptoServiceProvider tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider();
            byte[] numArray = new byte[hash.Length / 2];
            for (int i = 0; i < hash.Length / 2; i++)
            {
                int ınt32 = Convert.ToInt32(hash.Substring(i * 2, 2), 16);
                numArray[i] = (byte)ınt32;
            }
            PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(_encryptorKey, Encoding.ASCII.GetBytes(_saltPacket), _hashType, 1);
            tripleDesCryptoServiceProvider.Key = passwordDeriveByte.CryptDeriveKey(_algType, _hashType, 0, Encoding.ASCII.GetBytes(_ivString));
            tripleDesCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(_ivString);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(numArray, 0, (int)numArray.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public string Encrypt(string value)
        {
            TripleDESCryptoServiceProvider tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(_encryptorKey, Encoding.ASCII.GetBytes(_saltPacket), _hashType, 1);
            tripleDesCryptoServiceProvider.Key = passwordDeriveByte.CryptDeriveKey(_algType, _hashType, 0, Encoding.ASCII.GetBytes(_ivString));
            tripleDesCryptoServiceProvider.IV = Encoding.ASCII.GetBytes(_ivString);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, tripleDesCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, (int)bytes.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = memoryStream.ToArray();
            for (int i = 0; i < (int)array.Length; i++)
            {
                stringBuilder.AppendFormat("{0:X2}", array[i]);
            }
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
        }
    }
}
