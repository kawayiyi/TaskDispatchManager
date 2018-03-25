using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.Security.Cryptography;

namespace TaskDispatchManager.Common
{
    public class CodingUtils
    {
        #region

        /// <summary>
        /// 对字符串进行自定义格式加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EncryptCarKeys(string source)
        {
            string key0 = "CarKeys"; //加点盐
            source = string.Format("{0}{1}", source, key0);
            return SHA256(source);
        }

        #endregion

        #region SHA1

        public static string GetSwcSH1(string value)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder sh1 = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sh1.Append(data[i].ToString("x2").ToUpperInvariant());
            }
            return sh1.ToString();
        }

        #endregion

        #region SHA256

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }

        #endregion

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static string SHA512(string source)
        {
            string result = string.Empty;
            SHA512 sha512 = new SHA512Managed();
            byte[] s = sha512.ComputeHash(Encoding.UTF8.GetBytes(source));
            for (int i = 0; i < s.Length; i++)
            {
                result += s[i].ToString("X");
            }
            sha512.Clear();
            return result;
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = string.Empty;
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }

        #region AES

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
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(@"F30F6FD087514424B671C397AF1C1C50");

                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

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

                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(@"F30F6FD087514424B671C397AF1C1C50");

                byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion


    }
}
