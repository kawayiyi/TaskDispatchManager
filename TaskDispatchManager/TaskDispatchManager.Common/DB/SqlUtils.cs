using System;
using System.Collections.Generic;
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
using Microsoft.VisualBasic;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// SQL字符串相关工具类
    /// </summary>
    public class SqlUtils
    {
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="sql">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string sql)
        {
            return !Regex.IsMatch(sql, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 删除SQL注入特殊字符  
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public static string StripSQLInjection(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                //过滤 ' --  
                string pattern1 = @"(\%27)|(\')|(\-\-)";

                //防止执行 ' or  
                string pattern2 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";

                //防止执行sql server 内部存储过程或扩展存储过程  
                string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";

                sql = Regex.Replace(sql, pattern1, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern2, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern3, string.Empty, RegexOptions.IgnoreCase);
            }
            return sql;
        }

        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public static string MashSQL(string sql)
        {
            return (string.IsNullOrEmpty(sql)) ? string.Empty : sql.Replace("\'", "'");
        }

        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public static string ChkSQL(string sql)
        {
            return (string.IsNullOrEmpty(sql)) ? string.Empty : sql.Replace("'", "''");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string SQLSafe(string parameter)
        {
            parameter = parameter.ToLower();
            parameter = parameter.Replace("'", "");
            parameter = parameter.Replace(">", ">");
            parameter = parameter.Replace("<", "<");
            parameter = parameter.Replace("\n", "<br>");
            parameter = parameter.Replace("\0", "·");
            return parameter;
        }
    }
}
