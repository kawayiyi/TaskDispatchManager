using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// 安全检查（防止XSS跨站脚本攻击）
    /// </summary>
    /// <remarks>冯瑞 2013-3-4</remarks>
    public class SafeUtils
    {
        /// <summary>
        /// 非法数据定义 GET
        /// </summary>
        private const string getRegex = "<|>|\"|'|\\b(and|or)\\b.+?(>|<|=|\\bin\\b|\\blike\\b)|\\/\\*.+?\\*\\/|<\\s*script\\b|\\bEXEC\\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\\s+(TABLE|DATABASE)";
        /// <summary>
        /// 非法数据定义 POST
        /// </summary>
        private const string postRegex = "\\b(and|or)\\b.{1,6}?(=|>|<|\\bin\\b|\\blike\\b)|\\/\\*.+?\\*\\/|<\\s*script\\b|\\bEXEC\\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\\s+(TABLE|DATABASE)";
        /// <summary>
        /// 非法数据定义 COOKIE
        /// </summary>
        private const string cookieRegex = "\\b(and|or)\\b.{1,6}?(=|>|<|\\bin\\b|\\blike\\b)|\\/\\*.+?\\*\\/|<\\s*script\\b|\\bEXEC\\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\\s+(TABLE|DATABASE)";
        /// <summary>
        /// 检测POST
        /// </summary>
        /// <returns></returns>
        public static bool PostData()
        {
            bool result = false;

            if (HttpContext.Current == null) return result;

            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.Form[i].ToString(), postRegex);
                if (result)
                {
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 检查QUERY
        /// </summary>
        /// <returns></returns>
        public static bool GetData()
        {
            bool result = false;

            if (HttpContext.Current == null) return result;

            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = CheckData(HttpContext.Current.Request.QueryString[i].ToString(), getRegex);
                if (result)
                {
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 检测COOKIE
        /// </summary>
        /// <returns></returns>
        public static bool CookieData()
        {
            bool result = false;

            if (HttpContext.Current == null) return result;

            try
            {
                for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
                {
                    if (HttpContext.Current.Request.Cookies[i] == null) continue;

                    result = CheckData(HttpContext.Current.Request.Cookies[i].Value.ToLower(), cookieRegex);
                    if (result)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                //Utils.WriteErrorLog("检测COOKIE数据异常" + ex.Message, ex);
            }
            return result;

        }
        /// <summary>
        /// 检测来源
        /// </summary>
        /// <returns></returns>
        public static bool referer()
        {
            bool result = false;
            return result = CheckData(HttpContext.Current.Request.UrlReferrer.ToString(), getRegex);
        }
        /// <summary>
        /// 检测数据
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool CheckData(string inputData, string regex)
        {
            if (Regex.IsMatch(inputData, regex))
            {
                //Utils.WriteErrorLog(WebRequest.GetIP() + " 提交中有非法数据 " + inputData);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
