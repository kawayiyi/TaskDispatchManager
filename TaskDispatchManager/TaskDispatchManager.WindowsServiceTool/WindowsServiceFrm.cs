using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskDispatchManager.WindowsServiceTool
{
    public partial class FrmMain : Form
    {
        public FrmMain(string[] args)
        {
            InitializeComponent();
            if (args != null && args.Length > 0)
            {
                txtPath.Text = args[0];
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = @"(*.exe)|*.exe";
            openFileDialog1.FileName = "TaskDispatchManager.WindowsService.exe";
            openFileDialog1.Title = @"选择Windows服务";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openFileDialog1.FileName;
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (!Vaild())
            {
                return;
            }
            try
            {
                string[] cmdline = { };
                string serviceFileName = txtPath.Text.Trim();
                string serviceName = GetServiceName(serviceFileName);
                if (string.IsNullOrEmpty(serviceName))
                {
                    txtTip.Text = @"指定文件不是Windows服务！";
                    return;
                }
                if (ServiceIsExisted(serviceName))
                {
                    txtTip.Text = @"要安装的服务已经存在！";
                    return;
                }
                TransactedInstaller transactedInstaller = new TransactedInstaller();
                AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline)
                {
                    UseNewContext = true
                };
                transactedInstaller.Installers.Add(assemblyInstaller);
                transactedInstaller.Install(new System.Collections.Hashtable());
                txtTip.Text = @"服务安装成功！";
            }
            catch (Exception ex)
            {
                txtTip.Text = ex.Message;
            }
        }





        /// <summary>
        /// 操作前校验
        /// </summary>
        /// <returns></returns>
        private bool Vaild()
        {
            if (String.IsNullOrEmpty(txtPath.Text.Trim()))
            {
                txtTip.Text = @"请先选择Windows服务路径！";
                return false;
            }
            if (!File.Exists(txtPath.Text.Trim()))
            {
                txtTip.Text = @"路径不存在！";
                return false;
            }
            if (!txtPath.Text.Trim().EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                txtTip.Text = @"所选文件不是Windows服务！";
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取Windows服务的名称
        /// </summary>
        /// <param name="serviceFileName">文件路径</param>
        /// <returns>服务名称</returns>
        private string GetServiceName(string serviceFileName)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(serviceFileName);
                Type[] types = assembly.GetTypes();
                foreach (Type myType in types)
                {
                    if (myType.IsClass && myType.BaseType == typeof(System.Configuration.Install.Installer))
                    {
                        FieldInfo[] fieldInfos = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Default | BindingFlags.Instance | BindingFlags.Static);
                        foreach (FieldInfo myFieldInfo in fieldInfos)
                        {
                            if (myFieldInfo.FieldType == typeof(System.ServiceProcess.ServiceInstaller))
                            {
                                ServiceInstaller serviceInstaller = (ServiceInstaller)myFieldInfo.GetValue(Activator.CreateInstance(myType));
                                return serviceInstaller.ServiceName;
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 判断服务是否已经存在
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>bool</returns>
        private bool ServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Any(s => s.ServiceName == serviceName);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!Vaild())
            {
                return;
            }
            try
            {
                string serviceName = GetServiceName(txtPath.Text.Trim());
                if (string.IsNullOrEmpty(serviceName))
                {
                    txtTip.Text = @"指定文件不是Windows服务！";
                    return;
                }
                if (!ServiceIsExisted(serviceName))
                {
                    txtTip.Text = @"要运行的服务不存在！";
                    return;
                }
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                    txtTip.Text = @"服务运行成功！";
                }
                else
                {
                    txtTip.Text = @"服务正在运行！";
                }
            }
            catch (Exception ex)
            {
                txtTip.Text = ex.Message;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!Vaild())
            {
                return;
            }
            try
            {
                string[] cmdline = { };
                string serviceFileName = txtPath.Text.Trim();
                string serviceName = GetServiceName(serviceFileName);
                if (string.IsNullOrEmpty(serviceName))
                {
                    txtTip.Text = @"指定文件不是Windows服务！";
                    return;
                }
                if (!ServiceIsExisted(serviceName))
                {
                    txtTip.Text = @"要停止的服务不存在！";
                    return;
                }
                ServiceController service = new ServiceController(serviceName);
                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    txtTip.Text = @"服务停止成功！";
                }
                else
                {
                    txtTip.Text = @"服务已经停止！";
                }

            }
            catch (Exception ex)
            {
                txtTip.Text = ex.Message;
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            if (!Vaild())
            {
                return;
            }
            try
            {
                string serviceFileName = txtPath.Text.Trim();
                string serviceName = GetServiceName(serviceFileName);
                if (string.IsNullOrEmpty(serviceName))
                {
                    txtTip.Text = @"指定文件不是Windows服务！";
                    return;
                }
                if (!ServiceIsExisted(serviceName))
                {
                    txtTip.Text = @"要卸载的服务不存在！";
                    return;
                }
                //让主线程去访问设置提示信息
                if (mbackgroundWorker.IsBusy)
                {
                    MessageBox.Show(@"当前进程正在生成脚本，请等待本次操作完成！");
                    return;
                }
                //后台运行卸载服务
                mbackgroundWorker.RunWorkerAsync(serviceFileName);
            }
            catch (Exception ex)
            {
                txtTip.Text = ex.Message;
            }
        }
    }
}
