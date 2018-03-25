using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDispatchManager.Common;
using TaskDispatchManager.Component;
using TaskDispatchManager.Component.Proxy;
using TaskDispatchManager.DBModels.Base;
using TaskDispatchManager.IService;
using TaskDispatchManager.ServiceModel;

namespace TaskDispatchManager.Service
{
    public partial class ProxyService
    {

        /// <summary>
        /// 从数据库里面拿出一个可以使用的代理ip
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetCorrectIp(ProxyParam param)
        {
            IProxyUseHistoryService proxyUseHistoryService = new ProxyUseHistoryService();
            string proxyIp = string.Empty;
            //当前页
            int currentPage = 1;
            int PageSize = 100;
            var proxyType = ProxyType.Http.GetDescription();
            var proxyJobType = ProxyUseForType.ProxyJob.GetDescription();
            var updateProxies = new List<Proxy>();

            while (string.IsNullOrEmpty(proxyIp))
            {
                int total;
                var proxyUnUseList = LoadPageEntities(currentPage * PageSize, (currentPage - 1) * PageSize + 1, out total, r => !r.IsDelete && r.Type.Equals(proxyType), o => o.Speed, true);
                var proxyUsedList = proxyUseHistoryService.LoadEntities(r=>r.Type.Equals(proxyJobType)).Select(p=>p.ProxyGuid);
                var proxyList = proxyUnUseList.Where(r => !proxyUsedList.Contains(r.Guid)).ToList();
                if (proxyList.Count == 0)
                {
                    break;
                }
                foreach (var item in proxyList)
                {
                    //检查是否能ping通并且可以代理拿到网页
                    if (WebUtils.PingProxy(item.IP, item.Port) && ProxyUtil.GetTotalPage(param.IpUrl,$"{item.IP}:{item.Port}")>1)
                    {
                        proxyIp = $"{item.IP}:{item.Port}";
                        proxyUseHistoryService.Add(new ProxyUseHistory() { Guid=Guid.NewGuid(), ProxyGuid = item.Guid,CreatedOn = DateTime.Now,Type = proxyJobType });
                        break;
                    }
                    else
                    {
                        item.IsDelete = true;
                        updateProxies.Add(item);

                    }
                }

                currentPage++;
            }

            //将不能使用的Ip删除
            if (updateProxies.Count>0)
            {
                Update(updateProxies,r=>r.IsDelete);
            }
            if (string.IsNullOrEmpty(proxyIp))
            {
                proxyIp = param.DefaultProxyIp;
            }
            return proxyIp;
        }
    }
}
