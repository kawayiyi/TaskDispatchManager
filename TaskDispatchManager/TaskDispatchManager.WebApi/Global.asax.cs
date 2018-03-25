using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json;

namespace TaskDispatchManager.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            #region 设置序列化
            //修正 ASP.NET Web API 的 XmlFormatter 所支持的媒体类型(MediaType) , 目的是不默认输出XML数据，而输出Json.

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            //设置序列化成JSON时，日期时间的格式 Dennis Feng by 2014-05-30
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings =
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    Culture = CultureInfo.GetCultureInfo("zh-CN"),
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                };

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CompressionHandler());

            #endregion



            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        /// <summary>
        /// Application_End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <reremarks>Dennis Feng by 2014-05-07</reremarks>
        protected void Application_End(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// 在接收到一个应用程序请求时触发。对于一个请求来说，它是第一个被触发的事件，请求一般是用户输入的一个页面请求（URL）。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <reremarks>Dennis Feng by 2014-05-07</reremarks>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region Request Log

            StringBuilder sbText = new StringBuilder();
            string url = string.Empty;
            StringBuilder requestTypeString = new StringBuilder();

            var nv = new NameValueCollection();
            if (HttpContext.Current.Request.RequestType.ToUpper() == "POST") //POST
            {
                nv = HttpContext.Current.Request.Form;
                url = Request.Url + "?" + Common.WebUtils.ToQueryString(nv);
                requestTypeString.Append("发生一个Post请求，如下：");

                if (HttpContext.Current.Request.InputStream.Length > 0)
                {
                    byte[] byts = new byte[Request.InputStream.Length];
                    HttpContext.Current.Request.InputStream.Read(byts, 0, byts.Length);
                    string json = System.Text.Encoding.UTF8.GetString(byts);

                    requestTypeString.Append("\r\n");
                    requestTypeString.Append("JSON数据如下：\r\n");
                    requestTypeString.Append(json);

                    //Dennis Feng by 2014-05-14 重要，必须，因为API会重新读取InputStream，所以这里复位。 
                    HttpContext.Current.Request.InputStream.Position = 0;
                }
            }
            else //GET
            {
                url = Request.Url.ToString();
                requestTypeString.Append("发生一个Get请求，如下：");
            }

            sbText.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            sbText.Append(requestTypeString);
            sbText.Append("\r\n");
            sbText.Append("--------------------------------------------------------------------------------");
            sbText.Append("\r\n");
            sbText.Append("客户机IP：");
            sbText.Append(Common.WebUtils.GetIP());
            sbText.Append("\r\n请求URL：");
            sbText.Append(url);
            sbText.Append("\r\n上一个URL：");
            sbText.Append(Request.UrlReferrer?.AbsoluteUri ?? "");
            sbText.Append("\r\n");

            Common.LogHelper.WriteRequestLog(sbText.ToString());

            #endregion

            #region 检测非法数据 （防止XSS跨站脚本攻击

            //遇到非法数据，不做处理，只做记录
            string q = "<div style='position:fixed;top:0px;width:100%;height:100%;background-color:white;color:green;font-weight:bold;border-bottom:5px solid #999;'><br>您的本次提交带有不合法参数，请<a href=\"http://www.baidu.com\" target=\"top\">返回</a>，谢谢合作！</div>";
            //安全检测，为TRUE则重写页面，否则只记录 ERRORLOG
            bool reWritePage = true;//Common.Utils.TypeConvertUtils.StrToInt(Common.Utils.ConfigUtils.GetConfigAppSettingsValueByKey("SafeCheck"), 0) > 0;

            if (Request.Cookies != null)
            {
                if (Common.SafeUtils.CookieData() && reWritePage)
                {
                    Response.Write(q);
                    Response.StatusCode = 403;
                    Response.End();
                }
            }
            if (Request.UrlReferrer != null)
            {
                if (Common.SafeUtils.referer() && reWritePage)
                {
                    Response.Write(q);
                    Response.StatusCode = 403;
                    Response.End();
                }
            }
            if (Request.RequestType.ToUpper() == "POST")
            {
                if (Common.SafeUtils.PostData() && reWritePage)
                {
                    Response.Write(q);
                    Response.StatusCode = 403;
                    Response.End();
                }
            }
            if (Request.RequestType.ToUpper() == "GET")
            {
                if (Common.SafeUtils.GetData() && reWritePage)
                {
                    Response.Write(q);
                    Response.StatusCode = 403;
                    Response.End();
                }
            }

            #endregion

            #region Gzip

            //var context = HttpContext.Current;
            //var request = context.Request;
            //var response = context.Response;
            //ResponseCompressionType compressionType = this.GetCompressionMode(request);

            //if (compressionType != ResponseCompressionType.None)
            //{
            //    response.AppendHeader("Content-Encoding", compressionType.ToString().ToLower());
            //    if (compressionType == ResponseCompressionType.GZip)
            //    {
            //        response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            //    }
            //    else
            //    {
            //        response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            //    }
            //}

            //#endregion

            //StringBuilder sbMsg = new StringBuilder();

            //sbMsg.Append("客户机IP：");
            //sbMsg.Append(Common.Utils.WebUtils.GetIP());
            //sbMsg.Append("\r\n请求的URL：");
            //sbMsg.Append(Request.Url);
            //sbMsg.Append("\r\n上一个URL：");
            //sbMsg.Append((Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : ""));
            //sbMsg.Append("\r\n");

            //Common.Log4NetHelper.WriteErrorLog(sbMsg.ToString());

            #endregion
        }


        /// <summary>
        /// 当应用程序中遇到一个未处理的异常时，该事件被触发。（事件显示一个简单的消息用以说明发生的错误。 ）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <reremarks>Dennis Feng by 2014-05-07</reremarks>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exp = HttpContext.Current.Server.GetLastError();

            StringBuilder sbErrorMsg = new StringBuilder();
            sbErrorMsg.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            sbErrorMsg.Append(" 发生一个系统错误，如下：");
            sbErrorMsg.Append("\r\n");
            sbErrorMsg.Append("--------------------------------------------------------------------------------");
            sbErrorMsg.Append("\r\n");

            sbErrorMsg.Append("客户机IP：");
            sbErrorMsg.Append(Common.WebUtils.GetIP());
            sbErrorMsg.Append("\r\n错误地址：");
            sbErrorMsg.Append(Request.Url);
            sbErrorMsg.Append("\r\n上一个URL：");
            sbErrorMsg.Append(Request.UrlReferrer?.AbsoluteUri ?? "");
            sbErrorMsg.Append("\r\n");

            HttpException httpEx = exp as HttpException;
            int httpCode = 0;
            if (null != httpEx)
            {
                httpCode = httpEx.GetHttpCode();
            }

            if (httpCode != 404) //如果HTTP状态码不等于404，并且有异常就记录日志(因为某些图片找不到，也算一个异常写进日志了，此判断看自己是否有需要)
            {
                //Common.Log4NetHelper.WriteErrorLog(sbErrorMsg.ToString(), exp);
            }

            //WriteErrorLog1(sbErrorMsg.ToString(), exp);
            Common.LogHelper.WriteErrorLog(sbErrorMsg.ToString(), exp);

        }
    }
}
