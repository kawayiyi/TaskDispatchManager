
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskDispatchManager.DBModels.Base;

namespace TaskDispatchManager.IService
{
    public partial interface IExpressCompanyService:IBaseService<ExpressCompany>
    {
    }

    public partial interface IExpressHistoryService:IBaseService<ExpressHistory>
    {
    }

    public partial interface IExpressService:IBaseService<Express>
    {
    }

    public partial interface IExpressProcessDetailService:IBaseService<ExpressProcessDetail>
    {
    }

    public partial interface IMessageService:IBaseService<Message>
    {
    }

    public partial interface IMessageHistoryService:IBaseService<MessageHistory>
    {
    }

    public partial interface IProxyService:IBaseService<Proxy>
    {
    }

    public partial interface IProxyUseHistoryService:IBaseService<ProxyUseHistory>
    {
    }

}

