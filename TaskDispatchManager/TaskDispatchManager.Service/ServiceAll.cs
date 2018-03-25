
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskDispatchManager.DBModels.Base;
using TaskDispatchManager.IService;

namespace TaskDispatchManager.Service
{
    public partial class ExpressCompanyService:BaseService<ExpressCompany>,IExpressCompanyService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ExpressCompanyDal;
		}
    }

    public partial class ExpressHistoryService:BaseService<ExpressHistory>,IExpressHistoryService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ExpressHistoryDal;
		}
    }

    public partial class ExpressService:BaseService<Express>,IExpressService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ExpressDal;
		}
    }

    public partial class ExpressProcessDetailService:BaseService<ExpressProcessDetail>,IExpressProcessDetailService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ExpressProcessDetailDal;
		}
    }

    public partial class MessageService:BaseService<Message>,IMessageService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.MessageDal;
		}
    }

    public partial class MessageHistoryService:BaseService<MessageHistory>,IMessageHistoryService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.MessageHistoryDal;
		}
    }

    public partial class ProxyService:BaseService<Proxy>,IProxyService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ProxyDal;
		}
    }

    public partial class ProxyUseHistoryService:BaseService<ProxyUseHistory>,IProxyUseHistoryService
    {
		public override void SetCurrentDal()
		{
			CurrentDal = DbSession.ProxyUseHistoryDal;
		}
    }

}

