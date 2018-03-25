
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskDispatchManager.DAL;
using TaskDispatchManager.IDAL;


namespace TaskDispatchManager.DalFactory
{
    /// <summary>
    /// DbSession:本质就是一个简单工厂，就是这么一个简单工厂，但从抽象意义上来说，它就是整个数据库访问层的统一入口。
    /// 因为拿到了DbSession就能够拿到整个数据库访问层所有的dal。
    /// </summary>
    public partial class DbSession :IDbSession
    { 

					private IExpressCompanyDal _ExpressCompanyDal;
          			public IExpressCompanyDal ExpressCompanyDal {
          				get {
          					if (_ExpressCompanyDal == null)
          					{
          						_ExpressCompanyDal = new ExpressCompanyDal();
          					}
          					return _ExpressCompanyDal;
          				}
          			}
          
          					private IExpressHistoryDal _ExpressHistoryDal;
          			public IExpressHistoryDal ExpressHistoryDal {
          				get {
          					if (_ExpressHistoryDal == null)
          					{
          						_ExpressHistoryDal = new ExpressHistoryDal();
          					}
          					return _ExpressHistoryDal;
          				}
          			}
          
          					private IExpressDal _ExpressDal;
          			public IExpressDal ExpressDal {
          				get {
          					if (_ExpressDal == null)
          					{
          						_ExpressDal = new ExpressDal();
          					}
          					return _ExpressDal;
          				}
          			}
          
          					private IExpressProcessDetailDal _ExpressProcessDetailDal;
          			public IExpressProcessDetailDal ExpressProcessDetailDal {
          				get {
          					if (_ExpressProcessDetailDal == null)
          					{
          						_ExpressProcessDetailDal = new ExpressProcessDetailDal();
          					}
          					return _ExpressProcessDetailDal;
          				}
          			}
          
          					private IMessageDal _MessageDal;
          			public IMessageDal MessageDal {
          				get {
          					if (_MessageDal == null)
          					{
          						_MessageDal = new MessageDal();
          					}
          					return _MessageDal;
          				}
          			}
          
          					private IMessageHistoryDal _MessageHistoryDal;
          			public IMessageHistoryDal MessageHistoryDal {
          				get {
          					if (_MessageHistoryDal == null)
          					{
          						_MessageHistoryDal = new MessageHistoryDal();
          					}
          					return _MessageHistoryDal;
          				}
          			}
          
          					private IProxyDal _ProxyDal;
          			public IProxyDal ProxyDal {
          				get {
          					if (_ProxyDal == null)
          					{
          						_ProxyDal = new ProxyDal();
          					}
          					return _ProxyDal;
          				}
          			}
          
          					private IProxyUseHistoryDal _ProxyUseHistoryDal;
          			public IProxyUseHistoryDal ProxyUseHistoryDal {
          				get {
          					if (_ProxyUseHistoryDal == null)
          					{
          						_ProxyUseHistoryDal = new ProxyUseHistoryDal();
          					}
          					return _ProxyUseHistoryDal;
          				}
          			}
          
          		    }
}

