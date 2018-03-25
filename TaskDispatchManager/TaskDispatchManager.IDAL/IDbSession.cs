
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace TaskDispatchManager.IDAL
{

	public partial  interface  IDbSession
    {  
		int SaveChanges();

		IExpressCompanyDal  ExpressCompanyDal  { get;  }
          IExpressHistoryDal  ExpressHistoryDal  { get;  }
          IExpressDal  ExpressDal  { get;  }
          IExpressProcessDetailDal  ExpressProcessDetailDal  { get;  }
          IMessageDal  MessageDal  { get;  }
          IMessageHistoryDal  MessageHistoryDal  { get;  }
          IProxyDal  ProxyDal  { get;  }
          IProxyUseHistoryDal  ProxyUseHistoryDal  { get;  }
    }
}

