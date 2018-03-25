
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDispatchManager.DBModels.Base;

namespace TaskDispatchManager.IDAL
{
    public partial interface IExpressCompanyDal:IBaseDal<ExpressCompany>
    {
    }

    public partial interface IExpressHistoryDal:IBaseDal<ExpressHistory>
    {
    }

    public partial interface IExpressDal:IBaseDal<Express>
    {
    }

    public partial interface IExpressProcessDetailDal:IBaseDal<ExpressProcessDetail>
    {
    }

    public partial interface IMessageDal:IBaseDal<Message>
    {
    }

    public partial interface IMessageHistoryDal:IBaseDal<MessageHistory>
    {
    }

    public partial interface IProxyDal:IBaseDal<Proxy>
    {
    }

    public partial interface IProxyUseHistoryDal:IBaseDal<ProxyUseHistory>
    {
    }

}

