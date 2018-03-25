using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using TaskDispatchManager.Common;
using TaskDispatchManager.Component;
using TaskDispatchManager.Component.Proxy;
using TaskDispatchManager.DBModels.Base;
using TaskDispatchManager.IService;
using TaskDispatchManager.Service;
using TaskDispatchManager.ServiceModel;

namespace TaskDispatchManager.Tasks
{
    public class ProxyJob : IJob
    {
        /// <summary>
        ///任务是否正在执行标记 ：false--未执行； true--正在执行； 默认未执行
        /// </summary>
        private static bool _isRun = false;

        /// <summary>
        /// 任务总共执行次数
        /// </summary>
        private static int _executeCount = 0;

        /// <summary>
        /// 没执行5次任务切换代理IP
        /// </summary>
        private static int _speed = 5;

        /// <summary>
        /// 是否需要切换代理ip
        /// </summary>
        private static  bool _needChangeIp = false;

        /// <summary>
        /// 代理IP
        /// </summary>
        private static string _proxyIp;

        private readonly IProxyService _proxyService = new ProxyService();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                if (!_isRun)
                {
                    _isRun = true;
                    object objParam = context.JobDetail.JobDataMap.Get("TaskParam");
                    if (null != objParam)
                    {
                        ProxyParam param = new ToJson<ProxyParam>().FromJsonString(objParam.ToString());
                        DateTime start = DateTime.Now;
                        if (_needChangeIp || _executeCount % _speed == 0)
                        {
                            if (_needChangeIp)
                            {
                                _executeCount = (_executeCount / _speed + 1) * _speed;
                            }
                            //从数据库里面拿出一个可以使用的代理ip
                            _proxyIp = _proxyService.GetCorrectIp(param);
                            _needChangeIp = false;
                        }
                        param.ProxyIp = _proxyIp;
                        _executeCount++;
                        LogHelper.WriteInfoLog($"第{_executeCount}次爬虫开始");
                        List<Proxy> list = ProxyUtil.ParseProxy(param);
                        if (list.Count == 0)
                        {
                            //没有返回数据.表示当前IP已经被锁定需要更换
                            _needChangeIp = true;
                        }
                        else
                        {
                            _proxyService.Add(list);
                        }
                        LogHelper.WriteInfoLog($"第{_executeCount}次爬虫结束");
                    }
                    _isRun = false;
                }
            }
            catch (Exception ex)
            {

                JobExecutionException e2 = new JobExecutionException(ex);
                LogHelper.WriteErrorLog("爬虫获取代理ip任务异常", ex);
                _isRun = false;
                _executeCount++;
                //1.立即重新执行任务 
                e2.RefireImmediately = true;
                //2 立即停止所有相关这个任务的触发器
                //e2.UnscheduleAllTriggers=true; 
            }

        }



    }
}
