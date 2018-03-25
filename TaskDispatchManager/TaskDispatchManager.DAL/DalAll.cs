
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskDispatchManager.IDAL;
using TaskDispatchManager.DBModels.Base;

namespace TaskDispatchManager.DAL
{

    public partial class ExpressCompanyDal:BaseDal<ExpressCompany>,IExpressCompanyDal
    {
    }


    public partial class ExpressHistoryDal:BaseDal<ExpressHistory>,IExpressHistoryDal
    {
    }


    public partial class ExpressDal:BaseDal<Express>,IExpressDal
    {
    }


    public partial class ExpressProcessDetailDal:BaseDal<ExpressProcessDetail>,IExpressProcessDetailDal
    {
    }


    public partial class MessageDal:BaseDal<Message>,IMessageDal
    {
    }


    public partial class MessageHistoryDal:BaseDal<MessageHistory>,IMessageHistoryDal
    {
    }


    public partial class ProxyDal:BaseDal<Proxy>,IProxyDal
    {
    }


    public partial class ProxyUseHistoryDal:BaseDal<ProxyUseHistory>,IProxyUseHistoryDal
    {
    }

}
