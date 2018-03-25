using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.IDAL
{
    /// <summary>
    /// 抽象里所有的数据库访问层Dal实例的公共的方法约束。
    /// </summary>
    public interface IBaseDal<T> where T : class, new()
    {
        T Add(T entity);
        int Add(List<T> entityList);
        bool Update(T entity);
        bool Update(T entity, params Expression<Func<T, object>>[] properties);

        bool Update(List<T> entityList);

        bool Update(List<T> entityList, params Expression<Func<T, object>>[] properties);
        bool Delete(T entity);

        int Delete(params int[] ids);

        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);//规约设计模式。

        IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total,
                                                  Expression<Func<T, bool>> whereLambda
                                                  , Expression<Func<T, S>> orderbyLambda, bool isAsc);
    }
}
