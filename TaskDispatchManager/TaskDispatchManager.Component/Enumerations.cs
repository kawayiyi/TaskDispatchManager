using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.Component
{
    /// <summary>
    /// 代理的类型
    /// </summary>
    public enum ProxyType
    {
        [Description("HTTP")]
        Http=0,
        [Description("HTTPS")]
        Https=1,
        [Description("socks4/5")]
        Socks=2
    }


    /// <summary>
    /// 使用代理的类型
    /// </summary>
    public enum ProxyUseForType
    {
        [Description("ProxyJob")]
        ProxyJob = 0,
        [Description("HTTPS")]
        Https = 1,
        [Description("socks4/5")]
        Socks = 2
    }
}
