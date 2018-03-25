using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TaskDispatchManager.Common;

namespace TaskDispatchManager.WindowsService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
              
                TaskDispatchManagerService s = new TaskDispatchManagerService();
                string[] args = {"a", "b"};
                s.start(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new TaskDispatchManagerService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
