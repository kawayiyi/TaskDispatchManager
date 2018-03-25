using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// Log4Net封装
    /// </summary>
    public sealed class LogHelper
    {
       

        /// <summary>
        /// 日志器 级别 INFO
        /// </summary>
        private static readonly log4net.ILog Loginfo = log4net.LogManager.GetLogger("loginfo");

        /// <summary>
        /// 日志器 级别 ERROR
        /// </summary>
        private static readonly log4net.ILog Logerror = log4net.LogManager.GetLogger("logerror");

        /// <summary>
        /// 日志器 级别 TRACE
        /// </summary>
        private static readonly log4net.ILog Logtrace = log4net.LogManager.GetLogger("logtrace");

        /// <summary>
        /// 请求日志器 级别 Info
        /// </summary>
        private static readonly log4net.ILog Logrequest = log4net.LogManager.GetLogger("logrequest");

        /// <summary>
        /// 请求日志器 级别 Info
        /// </summary>
        private static readonly log4net.ILog Logquartnet = log4net.LogManager.GetLogger("logquartnet");

        /// <summary>
        /// 登录日志 级别 Info
        /// </summary>
        private static readonly log4net.ILog Loglogin = log4net.LogManager.GetLogger("loglogin");

        /// <summary>
        /// 控制台 级别
        /// </summary>
        /// <remarks>2013-8-14 BY Dennis Feng</remarks>
        private static readonly log4net.ILog Logconsole = log4net.LogManager.GetLogger("logconsole");

        /// <summary>
        /// 发送邮件 级别
        /// </summary>
        private static readonly log4net.ILog Logemail = log4net.LogManager.GetLogger("logemail");

        /// <summary>
        /// 发送DEBUG 级别
        /// </summary>
        private static readonly log4net.ILog Logdebug = log4net.LogManager.GetLogger("logdebug");


        private static readonly log4net.ILog Logsync = log4net.LogManager.GetLogger("logsync");
        /// <summary>
        /// 设置配置器入口
        /// </summary>
        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 设置配置器
        /// </summary>
        /// <param name="configFile"></param>
        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteErrorLog(string msg)
        {
            WriteErrorLog(msg, null);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteErrorLog(System.Exception ex)
        {
            WriteErrorLog(string.Empty, ex);
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void WriteErrorLog(string msg, System.Exception ex)
        {
            string methodName = GetMethodName((ex != null && !string.IsNullOrEmpty(msg)));

            StringBuilder temp = new StringBuilder();

            temp.Append(methodName);
            temp.Append(" ");
            temp.Append(msg);
            temp.Append(ex != null
                ? string.Format("{0}{1}{2}{3}", " 异常：", ex.Message, " ", (ex.InnerException != null
                                                    ? string.Format("{0}{1}", " 内部异常：", ex.InnerException.Message)
                                                    : ""))
                : "");

            msg = temp.ToString();

            WriteLog(msg, ex);
        }

        /// <summary>
        /// 得到调用方法名
        /// </summary>
        /// <param name="level">级别，true为2，false为3</param>
        /// <returns></returns>
        public static string GetMethodName(bool level)
        {

            var method = new StackFrame(level ? 2 : 3).GetMethod(); // 这里忽略2-3（看实际层级）层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            var property = (
                      from p in method.DeclaringType.GetProperties(
                               BindingFlags.Instance |
                               BindingFlags.Static |
                               BindingFlags.Public |
                               BindingFlags.NonPublic)
                      where p.GetGetMethod(true) == method || p.GetSetMethod(true) == method
                      select p).FirstOrDefault();
            return property == null ? method.Name : property.Name;
        }

        /// <summary>
        /// 调试输出
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteTraceLog(string msg)
        {
            if (LogHelper.Logtrace.IsInfoEnabled)
            {
                LogHelper.WriteLog(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// 写Info日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteInfoLog(string msg)
        {
            if (LogHelper.Loginfo.IsInfoEnabled)
            {
                Loginfo.Info(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteDebugLog(string msg)
        {
#if DEBUG
            StringBuilder temp = new StringBuilder();
            temp.Append(msg);
            temp.Append(Environment.NewLine);
            temp.Append("【Stack Message:】");
            temp.Append(Environment.NewLine);
            temp.Append(new StackTrace().ToString());
            msg = temp.ToString();

            if (LogHelper.Logdebug.IsDebugEnabled)
            {
                Logdebug.Debug(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
#endif
        }

        /// <summary>
        /// 写async日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteAsyncLog(string msg)
        {
            if (LogHelper.Logsync.IsInfoEnabled)
            {
                Logsync.Info(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }
        /// <summary>
        /// 写请求日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteRequestLog(string msg)
        {
            if (LogHelper.Logrequest.IsInfoEnabled)
            {
                Logrequest.Info(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// 写系统登录日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLoginLog(string msg)
        {
            if (LogHelper.Loglogin.IsInfoEnabled)
            {
                Loglogin.Info(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// 写QuartNET日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteQuartLog(string msg)
        {
            if (LogHelper.Logquartnet.IsInfoEnabled)
            {
                Logquartnet.Info(msg);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// 发送邮件  日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteEmailLog(string msg)
        {
            WriteEmailLog(msg,null);
        }
        public static void WriteEmailLog(Exception ex)
        {
            WriteEmailLog(null, ex);
        }
        public static void WriteEmailLog(string msg ,Exception ex)
        {
            string methodName = GetMethodName((ex != null && !string.IsNullOrEmpty(msg)));

            StringBuilder temp = new StringBuilder();

            temp.Append(methodName);
            temp.Append(" ");
            temp.Append(msg);
            temp.Append(ex != null
                ? string.Format("{0}{1}{2}{3}", " 异常：", ex.Message, " ", (ex.InnerException != null
                                                    ? string.Format("{0}{1}", " 内部异常：", ex.InnerException.Message)
                                                    : ""))
                : "");

            msg = temp.ToString();
            Logemail.Error(msg,ex);
        }

        #region private

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="info"></param>
        private static void WriteLog(string info)
        {
            if (Logtrace.IsInfoEnabled)
            {
                Logtrace.Info(info);
            }

            if (Logconsole.IsInfoEnabled)
            {
                Logconsole.Info(info);
            }

            //if (loginfo.IsInfoEnabled)
            //{
            //    loginfo.Info(info);
            //}
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        private static void WriteLog(string info, System.Exception se)
        {
            if (Logtrace.IsInfoEnabled)
            {
                Logtrace.Info(info);
            }

            if (Logconsole.IsInfoEnabled)
            {
                Logconsole.Info(info);
            }

            if (Logerror.IsErrorEnabled)
            {
                Logerror.Error(info, se);
            }
        }

        #endregion
    }
}
