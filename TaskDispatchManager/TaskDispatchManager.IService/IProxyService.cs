using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDispatchManager.ServiceModel;

namespace TaskDispatchManager.IService
{
    public partial interface IProxyService
    {
        string GetCorrectIp(ProxyParam param);
    }
}
