using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Vids.Service
{
    public class PasswordManager
    {      
        public static string HashUsingPbkdf2(string password, string salt)
        {
            using var bytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
            var derivedRandomKey = bytes.GetBytes(32);
            var hash = Convert.ToBase64String(derivedRandomKey);
            return hash;
        }

        public static string GenerateSalt(){
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string base64Salt = Convert.ToBase64String(salt);
            return base64Salt;
        }

        static byte[] TruncateHash(string key, int length)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                SHA1 sha1 = SHA1.Create();

                byte[] keyBytes = Encoding.Unicode.GetBytes(key);
                byte[] hash = sha1.ComputeHash(keyBytes);

                Array.Resize(ref hash, length);

                return hash;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        public static string EncryptPassword(string secret, string plainText)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                Byte[] plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                TripleDES tripleDES = TripleDES.Create();

                //TripleDESCryptoServiceProvider desCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDES.Key = TruncateHash(secret, tripleDES.KeySize / 8);
                tripleDES.IV = TruncateHash("", tripleDES.BlockSize / 8);

                CryptoStream cryptoStream = new CryptoStream(ms, tripleDES.CreateEncryptor(), CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return string.Empty;

            }
        }

        public static string DecryptPassword(string secret, string encryptedText)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                    return string.Empty;

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                TripleDES tripleDES = TripleDES.Create();

                //TripleDESCryptoServiceProvider desCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                tripleDES.Key = TruncateHash(secret, tripleDES.KeySize / 8);
                tripleDES.IV = TruncateHash("", tripleDES.BlockSize / 8);

                CryptoStream cryptoStream = new CryptoStream(ms, tripleDES.CreateDecryptor(), CryptoStreamMode.Write);

                cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                cryptoStream.FlushFinalBlock();

                return System.Text.Encoding.Unicode.GetString(ms.ToArray());

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return string.Empty;
            }
        }

    }
}