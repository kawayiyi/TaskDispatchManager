using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.DalFactory
{
    public class DbContextConfiguration : DbConfiguration
    {
        public DbContextConfiguration()
        {
            //SetDatabaseLogFormatter((context, action) => new SingleLineFormatter(context, action));
            this.AddInterceptor(new EntityFramworkCommandInterceptor());
        }
    }
}
