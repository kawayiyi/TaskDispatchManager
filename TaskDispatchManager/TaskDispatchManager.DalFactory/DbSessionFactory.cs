using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskDispatchManager.IDAL;

namespace TaskDispatchManager.DalFactory
{
    public class DbSessionFactory
    {
        public static IDAL.IDbSession GetCurrentDbSession()
        {

            IDbSession dbSession = (IDbSession)CallContext.GetData("DbSession");


            if (dbSession == null)
            {
                dbSession = new DbSession();
                CallContext.SetData("DbSession", dbSession);
            }


            return dbSession;
        }
    }
}
