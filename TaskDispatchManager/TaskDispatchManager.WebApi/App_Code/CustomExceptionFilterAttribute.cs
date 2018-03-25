using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace TaskDispatchManager.WebApi
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            base.OnException(context);

            var httpEx = context.Exception as HttpException;

            int httpCode = 0;

            if (null != httpEx)
            {
                httpCode = httpEx.GetHttpCode();
            }

            StringBuilder sbErrorMsg = new StringBuilder();
            sbErrorMsg.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            sbErrorMsg.Append(" 发生一个系统错误，如下：");
            sbErrorMsg.Append("\r\n");
            sbErrorMsg.Append("--------------------------------------------------------------------------------");
            sbErrorMsg.Append("\r\n");

            sbErrorMsg.Append("客户机IP：");
            sbErrorMsg.Append(Common.WebUtils.GetIP());
            sbErrorMsg.Append("\r\n错误地址：");
            sbErrorMsg.Append(context.Request.RequestUri);
            //sbErrorMsg.Append("\r\n上一个URL：");
            //sbErrorMsg.Append((context.ActionContext.Request != null ? Request.UrlReferrer.AbsoluteUri : ""));
            sbErrorMsg.Append("\r\n");

            //WriteErrorLog1(sbErrorMsg.ToString(), exp);
            Common.LogHelper.WriteErrorLog(sbErrorMsg.ToString(), httpEx);

            //Write Log
            //Common.Log4NetHelper.WriteErrorLog(exception.ThrowException);

            if (httpCode != (int)HttpStatusCode.NotFound) //如果HTTP状态码不等于404，并且有异常就记录日志(因为某些图片找不到，也算一个异常写进日志了，此判断看自己是否有需要)
            {
                Common.LogHelper.WriteErrorLog(context.Exception);
            }

            if (httpCode == 0)
            {
                httpCode = (int)HttpStatusCode.InternalServerError;
            }

            throw new HttpResponseException((HttpStatusCode)httpCode);
        }
    }
}