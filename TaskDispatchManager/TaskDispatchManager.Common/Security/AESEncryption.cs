using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// Implements AES-256 encryption/decryption
    /// </summary>
    public static class AesEncryption
    {
        #region Aes

        /*
         * AES简介

        AES（The Advanced Encryption Standard）是美国国家标准与技术研究所用于加密电子数据的规范。它被预期能成为人们公认的加密包括金融、电信和政府数字信息的方法。

        AES 是一个新的可以用于保护电子数据的加密算法。明确地说，AES 是一个迭代的、对称密钥分组的密码，它可以使用128、192 和 256 位密钥，并且用 128 位（16字节）分组加密和解密数据。与公共密钥密码使用密钥对不同，对称密钥密码使用相同的密钥加密和解密数据。通过分组密码返回的加密数据 的位数与输入数据相同。迭代加密使用一个循环结构，在该循环中重复置换（permutations ）和替换(substitutions）输入数据。Figure 1 显示了 AES 用192位密钥对一个16位字节数据块进行加密和解密的情形。
         * 
         * 
         * 
         */


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string AesEncrypt(string toEncrypt)
        {
            if (string.IsNullOrEmpty(toEncrypt))
            {
                return string.Empty;
            }
            try
            {
                var keyArray = Encoding.UTF8.GetBytes(@"BC6262A7963C4F86A6998935D9F28E82");

                var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

                var rDel = new RijndaelManaged
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                var cTransform = rDel.CreateEncryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string AesDecrypt(string toDecrypt)
        {
            if (string.IsNullOrEmpty(toDecrypt))
            {
                return string.Empty;
            }
            try
            {

                var keyArray = Encoding.UTF8.GetBytes(@"BC6262A7963C4F86A6998935D9F28E82");

                var toEncryptArray = Convert.FromBase64String(toDecrypt);

                var rDel = new RijndaelManaged
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                var cTransform = rDel.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return string.Empty;
            }
        }


        #endregion
    }
}
