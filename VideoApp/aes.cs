using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ezAcquire.Server.Extension.Utility
{
    public class Aes256Crypto 
    {
        #region<<Filed>>

        private readonly string _cryptoKey;

        #endregion

        #region <<Constructor>>

        public Aes256Crypto(string cryptoKey) {
            _cryptoKey = cryptoKey;
        }

        #endregion

        #region <<Private Method>>

        private Aes InitAes256() {
            Aes aes = Aes.Create();

            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.FeedbackSize = 8;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            using (SHA256 sha256 = SHA256.Create()) {
                aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(_cryptoKey));
            }

            return aes;
        }

        #endregion

        #region <<Public Method>>
        /// <summary>
        /// 將Byte加密
        /// </summary
        /// <param name="plainText">明碼Byte陣列</param>
        /// <returns>Base64字串</returns>
        public byte[] EncryptByteArray(Byte[] plainText) {
            byte[] encrypted;

            using (Aes aes256 = InitAes256()) {
                aes256.IV = Guid.NewGuid().ToByteArray();

                // First 16 bytes are IV
                encrypted = aes256.IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes256.CreateEncryptor();

                // Remain bytes are encrypted data
                encrypted = encrypted.Concat(encryptor.TransformFinalBlock(plainText, 0, plainText.Length).ToArray()).ToArray();
            }

            return encrypted;
        }

        /// <summary>
        /// 將字串加密成為Base64字串
        /// </summary
        /// <param name="plainText">明碼字串</param>
        /// <returns>Base64字串</returns>
        public string EncryptString(string plainText) {
            byte[] encrypted;

            using (Aes aes256 = InitAes256()) {
                aes256.IV = Guid.NewGuid().ToByteArray();

                // 將明碼字串用UTF8編碼成ByteArray
                var plainTextByteArray = Encoding.UTF8.GetBytes(plainText);

                // First 16 bytes are IV
                encrypted = aes256.IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes256.CreateEncryptor();

                // Remain bytes are encrypted data
                encrypted = encrypted.Concat(encryptor.TransformFinalBlock(plainTextByteArray, 0, plainTextByteArray.Length).ToArray()).ToArray();

            }

            return Convert.ToBase64String(encrypted).Replace('+', '-').Replace('/', '_');
        }

        /// <summary>
        /// 將加密後的Byte陣列解密
        /// </summary>
        /// <param name="cipherText">加密後的Byte陣列</param>
        /// <returns>明碼字串</returns>
        public byte[] DecryptByteArray(Byte[] cipherText) {
            byte[] decrypted;
            byte[] encrypted = cipherText;

            using (Aes aes256 = InitAes256()) {
                // Retrieve IV
                aes256.IV = encrypted.Take(16).ToArray();
                // Retrieve encrypted data
                encrypted = encrypted.Skip(16).ToArray();

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aes256.CreateDecryptor();
                decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
            }

            return decrypted;
        }

        /// <summary>
        /// 將加密後的Base64字串解密為明碼字串
        /// </summary>
        /// <param name="cipherText">加密後的String Base64字串</param>
        /// <returns>明碼字串</returns>
        public string DecryptString(string cipherText) {
            string plaintext = null;
            byte[] encrypted = Convert.FromBase64String(cipherText.Replace('_', '/').Replace('-', '+'));

            using (Aes aes256 = InitAes256()) {
                // Retrieve IV
                aes256.IV = encrypted.Take(16).ToArray();
                // Retrieve encrypted data
                encrypted = encrypted.Skip(16).ToArray();

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aes256.CreateDecryptor();

                // 解密並且將byte用UTF8編碼回來
                plaintext = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length));
            }

            return plaintext;
        }
        #endregion
    }
}
