using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.ServiceModel
{
    public class ProxyParam: ServiceModelBase
    {
        /// <summary>
        /// 提取代理ip站点地址
        /// </summary>
        public string IpUrl { get; set; }

        /// <summary>
        /// 请求站点默认使用的代理ip信息
        /// </summary>
        public string DefaultProxyIp { get; set; }

        /// <summary>
        /// 请求站点使用的代理ip信息
        /// </summary>
        public string ProxyIp { get; set; }

        /// <summary>
        /// 是否对获取的代理ip进行ping命令处理,确定该代理是否有效
        /// </summary>
        public bool IsPingIp { get; set; }

      
    }
}
