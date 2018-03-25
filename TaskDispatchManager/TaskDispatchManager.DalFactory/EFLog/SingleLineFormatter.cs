using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.DalFactory
{
    #region 更改Ef输出log的formatter
    /// <summary>
    /// 更改Ef输出log的formatter
    /// </summary>
    public class SingleLineFormatter : DatabaseLogFormatter
    {
        public SingleLineFormatter(DbContext ctx, Action<string> action)
       : base(ctx, action)
        {

        }
        public override void LogCommand<TResult>(System.Data.Common.DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            Write($"DbContext '{Context.GetType().Name}' - SQL:'{Environment.NewLine}' '{command.CommandText.Replace(Environment.NewLine, "")}' '{Environment.NewLine}'");

            //base.LogCommand<TResult>(command, interceptionContext);
        }
        public override void LogResult<TResult>(System.Data.Common.DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            //base.LogResult<TResult>(command, interceptionContext);
        }
    }


    #endregion




  
}
