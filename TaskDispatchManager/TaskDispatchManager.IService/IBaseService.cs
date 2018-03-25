using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.IService
{
    public interface IBaseService<T> where T : class, new()
    {

        T Add(T entity);
        int Add(List<T> entityList);
        bool Update(T entity);

        bool Update(T entity, params  Expression<Func<T, object>>[] properties);

        bool Update(List<T> entity);

        bool Update(List<T> entity, params Expression<Func<T, object>>[] properties);
        bool Delete(T entity);

        int Delete(params int[] ids);

        //u=>true
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);//规约设计模式。 

        IQueryable<T> LoadPageEntities<TS>(int pageSize, int pageIndex, out int total,
                                                  Expression<Func<T, bool>> whereLambda
                                                  , Expression<Func<T, TS>> orderbyLambda, bool isAsc);


        int Savechanges();
    }
}
