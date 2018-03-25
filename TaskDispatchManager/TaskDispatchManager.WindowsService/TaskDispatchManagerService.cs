using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskDispatchManager.Common;

namespace TaskDispatchManager.WindowsService
{
    public partial class TaskDispatchManagerService : ServiceBase
    {
        public TaskDispatchManagerService()
        {
            InitializeComponent();

            //string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //LogHelper.SetConfig(new System.IO.FileInfo(string.Concat(basePath, "log4net.config")));
            //加在log4
        }
        public void start(string[] args)
        {
            this.OnStart(args);
        }
        protected override void OnStart(string[] args)
        {
            //DebuggableAttribute att = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttribute<DebuggableAttribute>();
            //if (att.IsJITTrackingEnabled)
            //{
            //    //Debug模式才让线程停止10s,方便附加到进程调试
            //    Thread.Sleep(10000);
            //}
            //配置信息读取
            ConfigInit.InitConfig();
            QuartzHelper.InitScheduler();
            QuartzHelper.StartScheduler();
        }

        protected override void OnStop()
        {
            QuartzHelper.StopSchedule();
            System.Environment.Exit(0);
        }
    }
}
