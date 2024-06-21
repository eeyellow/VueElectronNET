using ElectronApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ElectronApp.Tools
{
    /// <summary>
    /// Enum 相關工具
    /// </summary>
    public static class EncryptTool
    {
        /// <summary> 加密金鑰 </summary>
        private static string _encryptKey = "";

        /// <summary> 設定加密金鑰 (預設會有KEY，但建議初始) </summary>
        /// <param name="key">加密KEY</param>
        public static void SetEncryptKey(string key) => _encryptKey = key;

        /// <summary> Aes加密 </summary>
        /// <param name="strInput">字串</param>
        /// <returns>加密後的字串</returns>
        public static string AesEncrypt(string strInput)
        {
            var encryptVal = string.Empty;
            try
            {
                var aes = Aes.Create();
                var md5 = MD5.Create();
                var sha256 = SHA256.Create();
                var key = sha256.ComputeHash(Encoding.UTF8.GetBytes(_encryptKey));
                var iv = md5.ComputeHash(Encoding.UTF8.GetBytes(_encryptKey));
                aes.Key = key;
                aes.IV = iv;

                var dataByteArray = Encoding.UTF8.GetBytes(strInput);
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encryptVal = Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                _ = e.Message;
            }
            return encryptVal;
        }

        /// <summary> Aes解密 </summary>
        /// <param name="strInput">字串</param>
        /// <returns>解密後的字串</returns>
        public static string AesDecrypt(string strInput)
        {
            var decryptVal = string.Empty;
            try
            {
                var aes = Aes.Create();
                var md5 = MD5.Create();
                var sha256 = SHA256.Create();
                var key = sha256.ComputeHash(Encoding.UTF8.GetBytes(_encryptKey));
                var iv = md5.ComputeHash(Encoding.UTF8.GetBytes(_encryptKey));
                aes.Key = key;
                aes.IV = iv;

                var dataByteArray = Convert.FromBase64String(strInput);
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                decryptVal = Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                _ = e.Message;
            }
            return decryptVal;
        }
    }
}
