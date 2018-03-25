using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.Service
{
    public abstract class BaseService<T> where T : class, new()
    {
        public IDAL.IDbSession DbSession => DalFactory.DbSessionFactory.GetCurrentDbSession();

        public IDAL.IBaseDal<T> CurrentDal;//依赖抽象的接口。


        public int Savechanges()
        {
            return DbSession.SaveChanges();
        }


        public virtual int Add(List<T> entityList)
        {
            CurrentDal.Add(entityList);

            return this.Savechanges();
        }


        //要求所有的业务方法在执行之前必须给当前的CurrentDal 赋值。

        //构造函数是在实例创建的时候就已经  执行了。
        protected BaseService()
        {
            //执行给当前CurrentDal赋值。
            //强迫子类给当前类的CurrentDal属性赋值。
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            SetCurrentDal();//调用了一个抽象方法。
        }

        //纯抽象方法：子类必须重写此方法。
        public abstract void SetCurrentDal();




        public virtual T Add(T entity)
        {
            //咱们的目的要拿到T对应的Dal
            //别用反射。


            return CurrentDal.Add(entity);


        }

        public virtual bool Update(T entity)
        {
            CurrentDal.Update(entity);

            return this.Savechanges() > 0;
        }

        public virtual bool Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            CurrentDal.Update(entity, properties);

            return this.Savechanges() > 0;
        }

        public virtual bool Update(List<T> entityList)
        {
            CurrentDal.Update(entityList);

            return this.Savechanges() > 0;
        }

        public virtual bool Update(List<T> entityList, params Expression<Func<T, object>>[] properties)
        {
            CurrentDal.Update(entityList, properties);

            return this.Savechanges() > 0;
        }

        public virtual bool Delete(T entity)
        {
            return CurrentDal.Delete(entity);

        }

        public virtual int Delete(params int[] ids)
        {
            return CurrentDal.Delete(ids);

        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);

        }

        public IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderbyLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities(pageSize, pageIndex, out total, whereLambda, orderbyLambda, isAsc);
        }

    }
}
