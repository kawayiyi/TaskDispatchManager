using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;
using System.IO;

using System.Net;
using System.Configuration;
using System;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// Config文件操作工具类
    /// </summary>
    /// <remarks>冯瑞 2011-09-29 17:49:05</remarks>
    public class ConfigUtils
    {
        /// <summary>
        /// 按键读取配置文件里AppSettings设置的该键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        /// <remarks>冯瑞 2011-10-09 09:30:37</remarks>
        public static string GetConfigAppSettingsValueByKey(string key)
        {
            string ret = string.Empty;
            try
            {
                ret = ConfigurationManager.AppSettings[key] != null ? ConfigurationManager.AppSettings[key].ToString().Trim() : string.Empty;
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// 按键读取配置文件里AppSettings设置的该键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="isThrowingExceptions">若找不到键值，是否引发异常？（true=是，false=否）</param>
        /// <returns></returns>
        /// <remarks>冯瑞 2011-10-09 09:30:37</remarks>
        public static string GetConfigAppSettingsValueByKey(string key, bool isThrowingExceptions)
        {
            string result = string.Empty;
            try
            {
                if (null != ConfigurationManager.AppSettings[key])
                {
                    result = ConfigurationManager.AppSettings[key].Trim();
                }
                else
                {
                    if (isThrowingExceptions)
                    {
                        throw new Exception(string.Format("没有在配置文件中的appSettings中找到{0}的配置，请检查配置文件配置！", key));
                    }
                    else
                    {
                        result = string.Empty;
                    }
                }
            }
            catch
            {
                throw new Exception(string.Format("读取配置文件的appSettings中的{0}键值时异常，请检查配置文件配置！", key));
            }
            return result;
        }

        /// <summary>
        /// 读取配置文件数据库链接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfigConnectionStringsValueByName(string name)
        {
            string ret = string.Empty;
            try
            {
                ret = ConfigurationManager.ConnectionStrings[name] != null ? ConfigurationManager.ConnectionStrings[name].ToString().Trim() : string.Empty;
            }
            catch
            {
                ret = string.Empty;
            }
            return ret;
        }

        /// <summary>
        /// 读取配置文件数据库链接字符串
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isThrowingExceptions">若找不到键值，是否引发异常？（true=是，false=否）</param>
        /// <returns></returns>
        public static string GetConfigConnectionStringsValueByName(string name, bool isThrowingExceptions)
        {
            var result = string.Empty;
            try
            {
                if (null != ConfigurationManager.ConnectionStrings[name])
                {
                    result = ConfigurationManager.ConnectionStrings[name].ToString().Trim();
                }
                else
                {
                    if (isThrowingExceptions)
                    {
                        throw new Exception(string.Format("没有在配置文件中的connectionStrings中找到{0}的配置，请检查配置文件配置！", name));
                    }
                    else
                    {
                        result = string.Empty;
                    }
                }
            }
            catch
            {
                if (isThrowingExceptions)
                {
                    throw new Exception(string.Format("没有在配置文件中的connectionStrings中找到{0}的配置，请检查配置文件配置！", name));
                }
            }
            return result;
        }
    }
}
