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

using System.Drawing;
using System.Drawing.Imaging;

using System.Net;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.UI;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// Web处理相关工具类
    /// </summary>
    /// <remarks>冯瑞 2011-09-29 17:44:57</remarks>
    public class WebUtils
    {
        #region Discuz原有方法

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return string.Empty;
            }

            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = string.Empty;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            return retVal;
        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }

            return request.Url.Host;
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得浏览器信息
        /// </summary>
        /// <returns></returns>
        public static string GetClientBrower()
        {
            string[] browerNames = { "MSIE", "Firefox", "Opera", "Netscape", "Safari", "Lynx", "Konqueror", "Mozilla" };

            string agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (!string.IsNullOrEmpty(agent))
            {
                foreach (string name in browerNames)
                {
                    if (agent.Contains(name))
                    {
                        return name;
                    }
                }
            }
            return "Other";
        }

        /// <summary>
        /// 判断当前客户端请求是否为IE
        /// </summary>
        /// <returns></returns>
        public static bool IsIE()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") >= 0;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }

            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName, false);
        }
        /// <summary>
        /// 获得指定Url参数的值
        /// </summary> 
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return string.Empty;
            }

            if (sqlSafeCheck && !SqlUtils.IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))
            {
                return "unsafe string";
            }

            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            return GetFormString(strName, false);
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return string.Empty;
            }

            if (sqlSafeCheck && !SqlUtils.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
            {
                return "unsafe string";
            }

            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            return GetString(strName, false);
        }

        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName, bool sqlSafeCheck)
        {
            string str = GetQueryString(strName);
            if (string.IsNullOrEmpty(str))
            {
                return GetFormString(strName, sqlSafeCheck);
            }
            else
            {
                return GetQueryString(strName, sqlSafeCheck);
            }
        }
        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName)
        {
            return TypeConvertUtils.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return TypeConvertUtils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return TypeConvertUtils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的long类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的long类型值</returns>
        public static long GetFormLong(string strName, long defValue)
        {
            return TypeConvertUtils.StrToLong(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url参数的long类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的long类型值</returns>
        public static long GetQueryLong(string strName, long defValue)
        {
            return TypeConvertUtils.StrToLong(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName, int defValue)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            else
            {
                return GetQueryInt(strName, defValue);
            }
        }

        /// <summary>
        /// 获得指定Url或表单参数的long类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的long类型值</returns>
        public static long GetLong(string strName, long defValue)
        {
            if (GetQueryLong(strName, defValue) == defValue)
            {
                return GetFormLong(strName, defValue);
            }
            else
            {
                return GetQueryLong(strName, defValue);
            }
        }

        /// <summary>
        /// 获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName, float defValue)
        {
            return TypeConvertUtils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        /// <summary>
        /// 获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName, float defValue)
        {
            return TypeConvertUtils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName, float defValue)
        {
            if (GetQueryFloat(strName, defValue) == defValue)
            {
                return GetFormFloat(strName, defValue);
            }
            else
            {
                return GetQueryFloat(strName, defValue);
            }
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result) || !ValidatorUtils.IsIPv4(result))
            {
                return "127.0.0.1";
            }

            return result;
        }

        /// <summary>
        /// 获得客户端外网IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static string GetClientInternetIP()
        {
            string ipAddress = string.Empty;
            using (WebClient webClient = new WebClient())
            {
                ipAddress = webClient.DownloadString("http://www.dxda.com/ip.asp");//站获得IP的网页
                //判断IP是否合法
                if (!System.Text.RegularExpressions.Regex.IsMatch(ipAddress, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
                {
                    ipAddress = webClient.DownloadString("http://www.zu14.cn/ip/");//站获得IP的网页
                }
            }
            return ipAddress;
        }

        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }

        #endregion

        #region 输出对象

        /// <summary>
        /// 向页面输出图片流,以Response方式向页面上输出Image对象的byte[]字节流
        /// </summary>
        /// <param name="img">图片对象(eg. System.Drawing.Image img = Image.FromFile("fileName"))</param>
        public static void OutPutImage(System.Drawing.Image img)
        {
            //转换成字节流
            byte[] b = ImagesUtils.ImageToByteArray(img);

            //清除该页输出缓存，设置该页无缓存
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Png";
            HttpContext.Current.Response.BinaryWrite(b.ToArray());
        }

        /// <summary>
        /// 向页面输出图片流,以Response方式向页面上输出Image对象的byte[]字节流
        /// </summary>
        /// <param name="fileName">图片文件完整路径（路径+文件名+扩展名）</param>
        public static void OutPutImage(string fileName)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(string.Format(@"{0}", fileName));
            OutPutImage(img);
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <remarks>Ralf 2013-01-10</remarks>
        public static void OutPutFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                //输出
                FileInfo fileInfo = new FileInfo(filePath);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileInfo.Name);
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
                System.Web.HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                System.Web.HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 当文档对象模型准备好后，输出脚本(基于jQuery)
        /// </summary>
        /// <param name="page">当前页面对象</param>
        /// <param name="script">脚本字符串（e.g: " alert('ok');  "）</param>
        /// <remarks>Ralf 2012-05-30</remarks>
        public static void WriteScriptWhenRocumentReady(System.Web.UI.Page page, string script)
        {
            string s = "<script type=\"text/javascript\"> jQuery(function(){ " + script + " ;});  </script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", s);
        }

        #endregion

        #region 操作 Cookies、Session

        /// <summary>
        /// 写cookie值（未设置过期时间，则写的是浏览器进程Cookie，一旦浏览器（是浏览器，非标签页）关闭，则Cookie自动失效）
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                cookie.HttpOnly = true;
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值（未设置过期时间，则写的是浏览器进程Cookie，一旦浏览器（是浏览器，非标签页）关闭，则Cookie自动失效）
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                cookie.HttpOnly = true;
            }
            cookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(毫秒),一旦设置过期时间，则认为是浏览器文件Cookie，而非浏览器进程Cookie</param>
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                cookie.HttpOnly = true;
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMilliseconds(expires);

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写Cookies数组
        /// </summary>
        /// <param name="groupName">Cookies数组对象名</param>
        /// <param name="dic">键值列表</param>
        /// <param name="timeoutSecond">超时时间（单位：秒）</param>
        /// <remarks>冯瑞 2011-11-02 16:52:06</remarks>
        public static void WriteCookieGroups(string groupName, Dictionary<String, String> dic, int timeoutSecond)
        {
            HttpCookie cookieGroup = new HttpCookie(groupName.Trim());
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            cookieGroup.HttpOnly = true;
            foreach (var Item in dic)
            {
                cookieGroup.Values[Item.Key] = Item.Value;
            }

            cookieGroup.Expires = DateTime.Now.AddSeconds(timeoutSecond);
            HttpContext.Current.Response.AppendCookie(cookieGroup);
        }

        #region 操作Cookie数组

        /// <summary>
        /// 读取Cookies数组
        /// </summary>
        /// <param name="groupName">Cookies数组对象名</param>
        /// <returns>Dictionary</returns>
        /// <remarks>冯瑞 2011-11-02 16:51:58</remarks>
        public static Dictionary<String, String> GetCookieGroups(string groupName)
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();

            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
            {
                if (HttpContext.Current.Request.Cookies.AllKeys[i] == groupName)
                {
                    HttpCookie CookieGroup = HttpContext.Current.Request.Cookies[groupName];
                    System.Collections.Specialized.NameValueCollection NameColl = CookieGroup.Values;
                    for (int j = 0; j < NameColl.Count; j++)
                    {
                        dic.Add(NameColl.AllKeys[j], NameColl[j]);
                    }
                    break; //此处防止有同名Cookies数组对象导致重复读取（加上break只读取同名Cookies的第一个Cookies数组对象）
                }
            }

            return dic;
        }

        /// <summary>
        /// 根据键值删除指定的Cookies数组里的键和值
        /// </summary>
        /// <param name="groupName">Cookies数组对象名</param>
        /// <param name="removeKey">要删除的键集合</param>
        /// <remarks> 冯瑞 2011-11-02 17:05:24</remarks>
        public static void RemoveGroupCookiesValue(string groupName, string[] removeKey)
        {
            HttpCookie acookie = HttpContext.Current.Request.Cookies[groupName];
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            acookie.HttpOnly = true;
            //得到原Cookie数组的过期时间
            RemoveGroupCookiesValue(groupName, removeKey, acookie.Expires);
        }

        /// <summary>
        /// 根据键值删除指定的Cookies数组里的键和值
        /// </summary>
        /// <param name="groupName">Cookies数组对象名</param>
        /// <param name="removeKey">要删除的键集合</param>
        /// <param name="expires">设置该Cookie数组的过期时间</param>
        /// <remarks>Ralf 2012-06-06</remarks>
        public static void RemoveGroupCookiesValue(string groupName, string[] removeKey, DateTime expires)
        {
            HttpCookie acookie = HttpContext.Current.Request.Cookies[groupName];
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            acookie.HttpOnly = true;
            acookie.Expires = expires; //设置过期时间
            if (acookie.HasKeys) //是否有子键
            {
                System.Collections.Specialized.NameValueCollection nameColl = acookie.Values;
                for (int i = 0; i < nameColl.Count; i++)
                {
                    for (int j = 0; j < removeKey.Length; j++)
                    {
                        if (nameColl.AllKeys[i] == removeKey[j])
                        {
                            acookie.Values.Remove(nameColl.AllKeys[i]);
                        }
                    }
                }
                HttpContext.Current.Response.Cookies.Add(acookie);
            }
        }

        /// <summary>
        /// 给指定的Cookies数组追加键值
        /// </summary>
        /// <param name="groupName">Cookies数组对象名</param>
        /// <param name="addDic">待追加的键值列表</param>
        public static void AddGroupCookiesValue(string groupName, Dictionary<String, String> addDic)
        {
            HttpCookie cookieGroup = HttpContext.Current.Request.Cookies[groupName];
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            cookieGroup.HttpOnly = true;
            //得到原Cookie数组的过期时间
            AddGroupCookiesValue(groupName, addDic, cookieGroup.Expires);
        }
        /// <summary>
        /// 给指定的Cookies数组追加键值
        /// </summary>
        /// <param name="groupName">Cookies数组对象名</param>
        /// <param name="addDic">待追加的键值列表</param>
        /// <param name="expires">设置该Cookie数组的过期时间</param>
        /// <remarks>Ralf 2012-06-06</remarks>
        public static void AddGroupCookiesValue(string groupName, Dictionary<String, String> addDic, DateTime expires)
        {
            HttpCookie cookieGroup = HttpContext.Current.Request.Cookies[groupName];
            cookieGroup.Expires = expires; //设置过期时间
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            cookieGroup.HttpOnly = true;

            foreach (var Item in addDic)
            {
                cookieGroup.Values[Item.Key] = Item.Value;
            }

            HttpContext.Current.Response.Cookies.Add(cookieGroup);
        }

        /// <summary>
        /// 修改指定的Cookies数组键值
        /// </summary>
        /// <param name="groupName">数组名</param>
        /// <param name="editDic">要修改的键值列表（只包含要修改的部分即可）</param>
        /// <remarks>Ralf 2012-06-06</remarks>
        public static void EditGroupCookiesValue(string groupName, Dictionary<String, String> editDic)
        {
            HttpCookie cookieGroup = HttpContext.Current.Request.Cookies[groupName];
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            cookieGroup.HttpOnly = true;
            //设置过期时间 （此处仍引用原Cookie的过期时间）
            EditGroupCookiesValue(groupName, editDic, cookieGroup.Expires);
        }
        /// <summary>
        /// 修改指定的Cookies数组键值
        /// </summary>
        /// <param name="groupName">数组名</param>
        /// <param name="editDic">要修改的键值列表（只包含要修改的部分即可）</param>
        /// <param name="expires">设置该Cookie数组的过期时间</param>
        /// <remarks>Ralf 2012-06-06</remarks>
        public static void EditGroupCookiesValue(string groupName, Dictionary<String, String> editDic, DateTime expires)
        {
            HttpCookie cookieGroup = HttpContext.Current.Request.Cookies[groupName];
            //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
            cookieGroup.HttpOnly = true;
            cookieGroup.Expires = expires; //设置过期时间

            System.Collections.Specialized.NameValueCollection nameColl = cookieGroup.Values;
            for (int i = 0; i < nameColl.Count; i++)
            {
                foreach (var Item in editDic)
                {
                    if (nameColl.AllKeys[i] == Item.Key) //如果找到匹配项
                    {
                        cookieGroup[nameColl.AllKeys[i]] = Item.Value;
                    }
                }
            }

            HttpContext.Current.Response.Cookies.Add(cookieGroup);
        }

        #endregion

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[strName][key].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 删除Cookies （可能有问题，可能不能删除全部Cookies，只能删除当前HttpRequest的Cookies）
        /// </summary>
        /// <param name="cookieName">Cookie名</param>
        public static void DelCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpCookie CookieObj = new HttpCookie(cookieName);
                CookieObj.Expires = DateTime.Now.AddDays(-1);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                CookieObj.HttpOnly = true;
                HttpContext.Current.Request.Cookies.Add(CookieObj);
            }
        }

        /// <summary>
        /// 删除指定HTTP请求的所有Cookie（一般用于退出登陆时调用）
        /// </summary>
        /// <param name="context"></param>
        public static void DelAllCookies(HttpContext context)
        {
            //Session.Clear();
            HttpCookie aCookie;
            string cookieName;
            int limit = context.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = context.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                aCookie.HttpOnly = true;
                context.Response.Cookies.Add(aCookie);
            }
            // context.Response.Redirect("/Default.aspx");
        }

        /// <summary>
        /// 得到Session
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>object</returns>
        /// <remarks>冯瑞 2011-11-03 16:41:04</remarks>
        public static object GetSessionReturnObject(string key)
        {
            if (HttpContext.Current.Session != null)
            {
                return HttpContext.Current.Session[key];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到Session
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>string</returns>
        /// <remarks>冯瑞 2011-11-03 16:42:25</remarks>
        public static string GetSessionReturnString(string key)
        {
            object obj = HttpContext.Current.Session[key];
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// 得到Session
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>object</returns>
        /// <remarks>冯瑞 2011-11-03 16:43:38</remarks>
        public static object GetSessionReturnObject(string key, object defaultValue)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                return defaultValue;
            }
            else
            {
                return HttpContext.Current.Session[key];
            }
        }
        /// <summary>
        /// 得到Session
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="canAdd"></param>
        /// <returns>object</returns>
        /// <remarks>冯瑞 2011-11-03 16:44:34</remarks>
        public static object GetSessionReturnObject(string key, object defaultValue, bool canAdd)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                if (canAdd)
                {
                    HttpContext.Current.Session.Add(key, defaultValue);
                }
                return defaultValue;
            }
            else
            {
                return HttpContext.Current.Session[key];
            }
        }

        /// <summary>
        /// 写Session
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">值</param>
        /// <returns>bool</returns>
        /// <remarks>冯瑞 2011-11-03 16:47:43</remarks>
        public static bool SetSession(string key, object value)
        {
            try
            {
                if (value == null && HttpContext.Current.Session[key] != null)
                {
                    HttpContext.Current.Session.Remove(key);
                }
                else if (HttpContext.Current.Session[key] == null)
                {
                    HttpContext.Current.Session.Add(key, value);
                }
                else
                {
                    HttpContext.Current.Session[key] = value;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 退出登录，删除所有Cookies
        /// </summary>
        /// <param name="context"></param>
        /// <remarks>Ralf 2012-07-05</remarks>
        public static void LoginOut(HttpContext context)
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = context.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = context.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                aCookie.HttpOnly = true;
                context.Response.Cookies.Add(aCookie);
            }
        }

        /// <summary>
        /// 退出登录，删除所有Cookies
        /// </summary>
        /// <param name="request"></param>
        public static void LoginOut(HttpRequest request)
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                aCookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }

        /// <summary>
        /// 退出登录，删除所有Cookies
        /// </summary>
        public static void LoginOut()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = HttpContext.Current.Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1);
                //cookie中设置了HttpOnly=true属性，那么通过js脚本将无法读取到cookie信息，这样能有效的防止XSS攻击，具体一点的介绍请google进行搜索 
                aCookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
        }

        #endregion

        #region 生成验证码

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <returns>返回生成的验证码字符(全部大写)</returns>
        /// <remarks>Ralf 2012-04-12</remarks>
        public static byte[] CreateVerificationImage(out string code)
        {
            //验证码
            string chkCode = string.Empty; //StringUtils.GetRandomChar(4);
            //颜色列表，用于验证码、噪线、噪点
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码
            string[] font = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //验证码的字符集，去掉了一些容易混淆的字符
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            Bitmap bmp = new Bitmap(100, 40);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线，循环次数越多，噪线越多
            for (int i = 0; i < 4; i++)
            {
                int x1 = rnd.Next(100);
                int y1 = rnd.Next(40);
                int x2 = rnd.Next(100);
                int y2 = rnd.Next(40);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }

            //画验证码字符串
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, 18);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 20 + 8, (float)8);
            }
            //画噪点，循环次数越多，噪点越多
            for (int i = 0; i < 50; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, bmp.Width - 1, bmp.Height - 1);

            //清除该页输出缓存，设置该页无缓存
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AppendHeader("Pragma", "No-Cache");
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出
            MemoryStream ms = new MemoryStream();

            code = chkCode;
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            finally
            {
                //显式释放资源
                bmp.Dispose();
                g.Dispose();
            }
        }

        #endregion

        #region URL、网站目录

        /// <summary>
        /// 获得Email的域名
        /// </summary>
        /// <param name="strEmail">Email</param>
        /// <returns></returns>
        /// <remarks>冯瑞 2011-11-02 17:35:17</remarks>
        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return string.Empty;
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        /// <param name="strHtmlPagePath"></param>
        /// <returns></returns>
        public string GetUrlDomainName(string strHtmlPagePath)
        {
            string p = @"http://[^\.]*\.(?<domain>[^/]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(strHtmlPagePath);
            return m.Groups["domain"].Value;
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 得到Web的真实路径
        /// </summary>
        /// <returns></returns>
        public static string GetWebPath()
        {
            string WebPath = HttpContext.Current.Request.Path;
            if (WebPath.LastIndexOf("/") != WebPath.IndexOf("/"))
            {
                WebPath = WebPath.Substring(WebPath.IndexOf("/"), WebPath.LastIndexOf("/") + 1);
            }
            else
            {
                WebPath = "/";
            }

            return WebPath;
        }

        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetFilename(string url)
        {
            if (url == null)
            {
                return string.Empty;
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }

        /// <summary>
        /// 得到网站的真实路径
        /// </summary>
        /// <returns></returns>
        public static string GetTrueSitePath()
        {
            string sitePath = HttpContext.Current.Request.Path;
            if (sitePath.LastIndexOf("/") != sitePath.IndexOf("/"))
            {
                sitePath = sitePath.Substring(sitePath.IndexOf("/"), sitePath.LastIndexOf("/") + 1);
            }
            else
            {
                sitePath = "/";
            }
            return sitePath;
        }

        /// <summary>
        /// 读取URL地址得到HTML
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetUrlContent(string url, Encoding encoding)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            try
            {
                return new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream(), encoding).ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>   
        /// 取得网站的根目录的URL   
        /// </summary>   
        /// <returns></returns>   
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                {
                    //直接安装在   Web   站点      
                    AppPath = UrlAuthority;
                }
                else
                {
                    //安装在虚拟子目录下
                    AppPath = UrlAuthority + Req.ApplicationPath;
                }
            }
            return AppPath;
        }

        /// <summary>   
        /// 取得网站的根目录的URL   
        /// </summary>   
        /// <param name="req"></param>   
        /// <returns></returns>   
        public static string GetRootURI(HttpRequest req)
        {
            string appPath = string.Empty;
            if (req != null)
            {
                string UrlAuthority = req.Url.GetLeftPart(UriPartial.Authority);
                if (req.ApplicationPath == null || req.ApplicationPath == "/")
                {
                    //直接安装在   Web   站点      
                    appPath = UrlAuthority;
                }
                else
                {
                    //安装在虚拟子目录下      
                    appPath = UrlAuthority + req.ApplicationPath;
                }
            }
            return appPath;
        }

        /// <summary>   
        /// 取得网站根目录的物理路径   
        /// </summary>   
        /// <returns></returns>   
        public static string GetRootPath()
        {
            string appPath = string.Empty;
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                appPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                appPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(appPath, @"\\$", RegexOptions.Compiled).Success)
                {
                    appPath = appPath.Substring(0, appPath.Length - 1);
                }
            }
            return appPath;
        }

        /// <summary>
        /// 获取站点根目录URL
        /// </summary>
        /// <returns></returns>
        public static string GetRootUrl(string forumPath)
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}",
                                 HttpContext.Current.Request.Url.Scheme,
                                 HttpContext.Current.Request.Url.Host.ToString(),
                                 (port == 80 || port == 0) ? "" : ":" + port,
                                 forumPath);
        }

        /// <summary>
        /// 本地路径转换成URL相对路径
        /// </summary>
        /// <param name="imagesurl1"></param>
        /// <returns></returns>
        public string URLConvertor(string imagesurl1)
        {
            HttpContext HttpCurrent = HttpContext.Current;
            string tmpRootDir = HttpCurrent.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            //imagesurl2 = imagesurl2.Replace(@"Aspx_Uc/", @"");
            return imagesurl2;
        }


        /// <summary>
        /// 相对路径转换成服务器本地物理路径
        /// </summary>
        /// <param name="imagesurl1"></param>
        /// <returns></returns>
        public string URLConvertorLocal(string imagesurl1)
        {
            HttpContext HttpCurrent = HttpContext.Current;
            string tmpRootDir = HttpCurrent.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = tmpRootDir + imagesurl1.Replace(@"/", @"\"); //转换成绝对路径
            return imagesurl2;
        }

        /// <summary>
        /// 对 URL 字符串进行编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 将已经为在 URL 中传输而编码的字符串转换为解码的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        #endregion

        #region 抓取页面

        /// <summary>
        /// 返回页面的响应信息
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static string GetWebResponse(string url)
        {
            try
            {
                Stream responseStream = WebRequest.Create(url).GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// Similar to GetWebPageResponse(string uriArg), but uses a user/pw to log in.
        /// </summary>
        /// <param name="uriArg">e.g. "http://192.168.2.1"</param>
        /// <param name="userArg">e.g. "root"</param>
        /// <param name="pwArg">e.g. "admin"</param>
        /// <returns>string containing the http response.</returns>
        /// <example>
        /// // Example to get a response with DHCP table from my LinkSys router.
        /// string s = GetWebPageResponse( "http://192.168.2.1/DHCPTable.htm", "root", "admin" );
        /// </example>
        static string GetWebPageResponse(string uriArg, string userArg, string pwArg)
        {
            Uri uri = new Uri(uriArg);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            CredentialCache creds = new CredentialCache();

            // See http://msdn.microsoft.com/en-us/library/system.directoryservices.protocols.authtype.aspx for list of types.
            const string authType = "basic";

            creds.Add(uri, authType, new NetworkCredential(userArg, pwArg));
            req.PreAuthenticate = true;
            req.Credentials = creds.GetCredential(uri, authType);

            Stream responseStream = req.GetResponse().GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            return reader.ReadToEnd();
        }

        #endregion

        #region 请求

        #region C#模拟http 发送post或get请求

        /**
         * 
         * 在post的时候有时也用的到cookie，像登录163发邮件时候就需要发送cookie，所以在外部一个cookie属性随时保存 CookieContainer cookie = new CookieContainer();

！注意：有时候请求会重定向，但我们就需要从重定向url获取东西，像QQ登录成功后获取sid，但上面的会自动根据重定向地址跳转。我们可以用:
request.AllowAutoRedirect = false;设置重定向禁用，你就可以从headers的Location属性中获取重定向地址
         * 
         */

        public static string PostHttpResponse(string url, Dictionary<string, string> parameters,
            int? timeout, string encode = "UTF-8")
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false; //是否使用 Nagle 不使用 提高效率
                //request.AllowWriteStreamBuffering = false; //数据是否缓冲 false 提高效率
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }
                else
                {
                    request.Timeout = 20000;
                }
                Encoding requestEncoding = Encoding.GetEncoding(encode);
                //如果需要POST数据  
                if (!(parameters == null || parameters.Count == 0))
                {

                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in parameters.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(parameters[key], requestEncoding));
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(parameters[key], requestEncoding));
                        }
                        i++;
                    }
                    byte[] data = requestEncoding.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                //获取响应，并设置响应编码
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string encoding = response.ContentEncoding;
                if (encoding == null || encoding.Length < 1)
                {
                    encoding = "UTF-8"; //默认编码
                }
                //读取响应流
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                string returnData = reader.ReadToEnd();
                reader.Dispose();
                response.Close();
                return returnData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string HttpPost(string Url, string postDataStr, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = contentType; //"application/x-www-form-urlencoded";
            //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string HttpGet(string Url, string postDataStr, string contentType = "text/html;charset=UTF-8")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = contentType; //"text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        #endregion

        /// <summary>
        /// 向指定的URL发送JSON字符串，并获得响应 (一般用于调用远程的ASP.NET Web API 接口，传递JSON数据)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        /// <reremarks>Dennis Feng by 2014-04-22</reremarks>
        public static string PostJson(string url, string jsonString)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = Encoding.UTF8.GetByteCount(jsonString);
            request.Timeout = 20000;

            HttpWebResponse response = null;

            try
            {
                StreamWriter swRequestWriter = new StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(jsonString);

                if (swRequestWriter != null)
                {
                    swRequestWriter.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }


        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHttpWebResponse(string url)
        {
            return GetHttpWebResponse(url, string.Empty);
        }

        /// <summary>
        /// http POST请求url
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="method_name"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetHttpWebResponse(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postData);
            request.Timeout = 20000;

            HttpWebResponse response = null;

            try
            {
                StreamWriter swRequestWriter = new StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(postData);

                if (swRequestWriter != null)
                {
                    swRequestWriter.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }


        //private string HttpPost(string Url, string postDataStr)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
        //    request.CookieContainer = cookie;
        //    Stream myRequestStream = request.GetRequestStream();
        //    StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
        //    myStreamWriter.Write(postDataStr);
        //    myStreamWriter.Close();

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    response.Cookies = cookie.GetCookies(response.ResponseUri);
        //    Stream myResponseStream = response.GetResponseStream();
        //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        //    string retString = myStreamReader.ReadToEnd();
        //    myStreamReader.Close();
        //    myResponseStream.Close();

        //    return retString;
        //}

        //public string HttpGet(string Url, string postDataStr)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
        //    request.Method = "GET";
        //    request.ContentType = "text/html;charset=UTF-8";

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    Stream myResponseStream = response.GetResponseStream();
        //    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        //    string retString = myStreamReader.ReadToEnd();
        //    myStreamReader.Close();
        //    myResponseStream.Close();

        //    return retString;
        //}

        /// <summary>
        /// hack tip:通过更新web.config文件方式来重启IIS进程池（注：iis中web园数量须大于1,且为非虚拟主机用户才可调用该方法）
        /// </summary>
        public static void RestartIISProcess()
        {
            try
            {
                System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
                xmldoc.Load(WebUtils.GetMapPath("~/web.config"));
                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(WebUtils.GetMapPath("~/web.config"), null);
                writer.Formatting = System.Xml.Formatting.Indented;
                xmldoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
            }
            catch
            {; }
        }

        #endregion

        #region 浏览器相关操作

        /// <summary>
        /// 调用IE浏览器打开网页(是在服务端打开)
        /// </summary>
        /// <param name="url">URL</param>
        /// <remarks>Ralf 2012-01-13</remarks>
        public static void OpenPageByIE(string url)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", url);
        }
        /// <summary>
        /// 获取客户端使用的平台名称
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static string GetBrowserPlatform()
        {
            return HttpContext.Current.Request.Browser.Platform.ToString();
        }
        /// <summary>
        /// 获取客户端浏览器的原始用户代理信息
        /// </summary>
        /// <returns></returns>
        public static string GetUserAgent()
        {
            return HttpContext.Current.Request.UserAgent.ToString();
        }
        /// <summary>
        /// 获取客户端语言首选项（即默认语言，e.g. zh-cn）
        /// </summary>
        /// <returns></returns>
        public static string GetUserLanguages()
        {
            return HttpContext.Current.Request.UserLanguages[0].ToString();
        }
        /// <summary>
        /// 获取远程客户端的DNS名称
        /// </summary>
        /// <returns></returns>
        public static string GetUserHostName()
        {
            return HttpContext.Current.Request.UserHostName.ToString();
        }
        /// <summary>
        /// 获取远程客户端主机IP
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static string GetUserHostAddress()
        {
            return HttpContext.Current.Request.UserHostAddress.ToString();
        }
        /// <summary>
        /// 得到浏览器主版本号
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static string GetBrowserMajorVersion()
        {
            return HttpContext.Current.Request.Browser.MajorVersion.ToString();
        }
        /// <summary>
        /// 获取浏览器的次（即小数）版本号
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static double GetBrowserMinorVersion()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.MinorVersion;
        }
        /// <summary>
        /// 获取浏览器的名称和主（整数）版本号 （e.g. IE9、Firefox11、Chrome17）
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static string GetBrowserName()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Type;
        }
        /// <summary>
        /// 获取浏览器类型名称（e.g. IE、Firefox、Chrome ...）
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserTypeName()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Browser;
        }
        /// <summary>
        /// 得到浏览器版本（e.g. 11.0、6.0、17.0）
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static string GetBrowserVersion()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Version;
        }
        /// <summary>
        /// 得到浏览器完整版本号
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static string GetBrowserVersion2()
        {
            return HttpContext.Current.Request.Browser.Version.ToString();
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否是Beta版
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static bool GetBrowserIsBeta()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Beta;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否为Web爬行遍历搜索引擎
        /// </summary>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-08</remarks>
        public static bool GetBrowserIsCrawler()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Crawler;
        }
        /// <summary>
        /// 获取一个值，该值指示客户端是否是“美国在线”（AOL）浏览器
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsAOL()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.AOL;
        }
        /// <summary>
        /// 获取一个值，该值指示客户端是否为基于Win32的计算机
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsWin32()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Win32;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持HTML框架
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsFrames()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Frames;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持HTML(table)元素
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsTables()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Tables;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持Cookie
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsCookies()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.Cookies;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持Visual Basic Script Edition（VBScript）
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsVBScript()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.VBScript;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持JavaScript
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsJavaScript()
        {
            //HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            //return bc.JavaScript;

            //上面的方法在新版.NET框架中提示已过时，所以采用下面的方法
            System.Web.HttpBrowserCapabilities myBrowserCaps = HttpContext.Current.Request.Browser;
            if (((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).EcmaScriptVersion.Major > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持Java
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsJavaApplets()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.JavaApplets;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持ActiveX控件
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsActiveXControls()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.ActiveXControls;
        }
        /// <summary>
        /// 获取一个值，该值指示浏览器是否支持Web广播的频道定义格式（CDF）
        /// </summary>
        /// <returns></returns>
        public static bool GetBrowserIsCDF()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.CDF;
        }
        /// <summary>
        /// 获取浏览器支持的万维网联合会（W3C）XML文档对象模型（DOM）的版本
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserW3CDomVersion()
        {
            HttpBrowserCapabilities bc = HttpContext.Current.Request.Browser;
            return bc.W3CDomVersion.ToString();
        }

        #endregion

        #region UBB

        /// <summary>
        /// 清空UBB
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks>2012-6-1 BY 田小岐</remarks>
        public static string ClearUBB(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            //过滤附件
            str = Regex.Replace(str, @"\[attachimg\].*\[/attachimg\]", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            //整理图片UBB
            str = Regex.Replace(str, @"\[img\](.*?)\[\/img\]", "<img src=\"$1\" border=\"0\" />", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\[img=(\d{1,9})[x|\,](\d{1,9})\](.*?)\[\/img\]", "<img src=\"$3\" width=\"$1\" height=\"$2\" border=\"0\" />", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"\[img=(\d{1,9})[x|\,](\d{1,9})[x|\,](\d+)\](.*?)\[\/img\]", "<img src=\"$4\" width=\"$1\" height=\"$2\" border=\"0\" aid=\"attachimg_$3\"/>", RegexOptions.IgnoreCase);

            //过滤UBB
            Regex regexBBS = new Regex(@"\[\/?\w+=?.*?\]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection matchsBBS = regexBBS.Matches(str);
            foreach (Match m in matchsBBS)
            {
                str = str.Replace(m.Value, string.Empty);
            }

            //过滤掉图片
            str = Regex.Replace(str, "<(img).*?(src=\"(.*)\").*?/>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return str;
        }

        #endregion

        #region 将键值对转换成URL参数

        /// <summary>
        /// 将键值对转换成URL参数
        /// </summary>
        /// <param name="nvc"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ToQueryString(NameValueCollection nvc, bool encode = true)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}",
                         encode ? HttpUtility.UrlEncode(key) : key
                         ,
                        encode ? HttpUtility.UrlEncode(value) : value))
                .ToArray();

            //升序
            var list = array.ToList();
            list = (from q in list orderby q select q).ToList();

            return string.Join("&", list);
        }

        #endregion

        #region 获取输入输出流

        /// <summary>
        /// 获得HttpContext.Current.Request.InputStream 数据返回string
        /// </summary>
        /// <returns></returns>
        /// <reremarks>Dennis Feng by 2014-05-14</reremarks>
        public static string GetInputStreamString()
        {
            string result = string.Empty;

            #region 写法1，这种方法也能读取

            //if (null != HttpContext.Current.Request)
            //{
            //    if (HttpContext.Current.Request.InputStream.Length > 0)
            //    {
            //        byte[] byts = new byte[HttpContext.Current.Request.InputStream.Length];
            //        HttpContext.Current.Request.InputStream.Read(byts, 0, byts.Length);
            //        result = System.Text.Encoding.UTF8.GetString(byts);
            //        HttpContext.Current.Request.InputStream.Position = 0;
            //    }
            //}

            #endregion

            #region 写法2

            if (null != HttpContext.Current.Request)
            {
                using (var s = new System.IO.StreamReader(HttpContext.Current.Request.InputStream))
                {
                    result = s.ReadToEnd();
                    //重要，这里如果不把Position置为0的话，下次HttpContext.Current.Request.InputStream将读取不到数据
                    HttpContext.Current.Request.InputStream.Position = 0;
                }
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 获得HttpContext.Current.Request.InputStream 数据返回System.IO.Stream
        /// </summary>
        /// <returns></returns>
        /// <reremarks>Dennis Feng by 2014-05-14</reremarks>
        public static System.IO.Stream GetInputStream()
        {
            using (System.IO.Stream result = HttpContext.Current.Request.InputStream)
            {
                //重要，这里如果不把Position置为0的话，下次HttpContext.Current.Request.InputStream将读取不到数据
                HttpContext.Current.Request.InputStream.Position = 0;
                return result;
            }
        }

        #endregion


        #region 网络协议
        /// <summary>  
        /// 是否能 Ping 通指定的主机  
        /// </summary>  
        /// <param name="ip">ip 地址或主机名或域名</param>  
        /// <returns>true 通，false 不通</returns>  
        public static bool Ping(string ip)
        {
            Ping p = new Ping();
            int timeout = 1000;
            PingReply reply = p.Send(ip, timeout);
            return reply != null && reply.Status == System.Net.NetworkInformation.IPStatus.Success;
        }
        public static bool PingProxy(string serverIp, string serverPort)
        {
            try
            {
                var mClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(serverIp);
                int iPortNo = System.Convert.ToInt16(serverPort);
                IPEndPoint ipEnd = new IPEndPoint(ip, iPortNo);
                mClientSocket.Connect(ipEnd, TimeSpan.FromSeconds(2));
                if (mClientSocket.Connected)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        #endregion
    }
}
