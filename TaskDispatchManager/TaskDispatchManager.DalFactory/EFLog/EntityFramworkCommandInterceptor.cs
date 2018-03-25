using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.DalFactory
{
    public class EntityFramworkCommandInterceptor : IDbCommandInterceptor
    {
        public void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //LogIfError(command, interceptionContext);
        }

        public void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //LogIfError(command, interceptionContext);
        }

        public void ReaderExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
            //LogIfError(command, interceptionContext);
        }

        public void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
            LogIfError(command, interceptionContext);
        }

        public void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            //LogIfError(command, interceptionContext);
        }

        public void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogIfError(command, interceptionContext);
        }


        private void LogIfError<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (interceptionContext.Exception != null)
            {
                Common.LogHelper.WriteErrorLog($" {Environment.NewLine} {GenSqlCmd(command.CommandText, command.Parameters)} failed with exception {interceptionContext.Exception}");
            }
            else
            {
                if (command.CommandType != CommandType.Text || !(command is SqlCommand))
                    return;
                Common.LogHelper.WriteDebugLog($" {Environment.NewLine}  {GenSqlCmd(command.CommandText, command.Parameters)}");
            }



            //if (command.CommandText.StartsWith("select", StringComparison.OrdinalIgnoreCase) && !command.CommandText.Contains("option(recompile)"))
            //{
            //    command.CommandText = command.CommandText + " option(recompile)";
            //}
        }


        public string GenSqlCmd(string inSqlCmd, DbParameterCollection p)
        {
            foreach (DbParameter x in p)
            {
                if (x.DbType == DbType.String || x.DbType== DbType.StringFixedLength )
                {
                    inSqlCmd = inSqlCmd.Replace($"@{x.ParameterName}" , $"'{x.Value}'");
                }
                else 
                {
                    inSqlCmd = inSqlCmd.Replace($"@{x.ParameterName}", $"{x.Value}");
                }
            }
            return inSqlCmd;
        }
    }
}
