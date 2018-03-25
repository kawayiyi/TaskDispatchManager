using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskDispatchManager.DAL
{
    /// <summary>
    /// 把数据库访问层公共的方法抽出来进行实现。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDal<T> where T : class, new()
    {
        //上下文直击New。那么这样不行。我们要保证db实例是线程内唯一。

        //问题：把保证Ef上下文实例唯一的代码写在这个地方合适吗？
        //考虑思路：第一当前类的职责是什么？当前的需求跟当前类的职责是否是一致的？
        //Model.DataModelContainer db =new DataModelContainer();

        private DbContext db => DbContextFactory.GetCurrentDbContext();

        public virtual T Add(T entity)
        {

            db.Set<T>().Add(entity);
            //db.SaveChanges();

            return entity;
        }

        public virtual int Add(List<T> entityList)
        {
            //可以不用附加：

            //db.Set<T>().Attach(entity); 内部就是只是把实体的 状态改成unchange跟数据库查询出来的状态时一样的。
            entityList?.ForEach(r =>
            {
                db.Set<T>().Add(r);
            });

            return entityList?.Count ?? 0;
        }

        public virtual bool Update(T entity)
        {
            //可以不用附加：

            //db.Set<T>().Attach(entity); 内部就是只是把实体的 状态改成unchange跟数据库查询出来的状态时一样的。
            db.Entry(entity).State = EntityState.Modified;
            //return db.SaveChanges() > 0;

            return true;
        }


        public virtual bool Update(T entity, params Expression<Func<T, object>>[] properties)
        {

            db.Entry(entity).State = EntityState.Unchanged;
            properties.ToList()
                 .ForEach((property) =>
                 {

                     Expression operand = ((UnaryExpression)property.Body).Operand;
                     string propertyName = ((MemberExpression)operand).Member.Name;
                     db.Entry(entity).Property(propertyName).IsModified = true;
                 });


            return true;
        }

        public virtual bool Update(List<T> entityList)
        {
            //可以不用附加：

            //db.Set<T>().Attach(entity); 内部就是只是把实体的 状态改成unchange跟数据库查询出来的状态时一样的。
            entityList?.ForEach(r =>
            {
                db.Entry(r).State = EntityState.Modified;
                //db.Entry(r).Property(property).IsModified = true;
            });


            return true;
        }

        public virtual bool Update(List<T> entityList, params Expression<Func<T, object>>[] properties)
        {
            //可以不用附加：

            //db.Set<T>().Attach(entity); 内部就是只是把实体的 状态改成unchange跟数据库查询出来的状态时一样的。
            entityList?.ForEach(r =>
            {
                db.Entry(r).State = EntityState.Unchanged;
                properties.ToList()
                .ForEach((property) =>
                {

                    Expression operand = ((UnaryExpression)property.Body).Operand;
                    string propertyName = ((MemberExpression)operand).Member.Name;
                    db.Entry(r).Property(propertyName).IsModified = true;
                });
            });


            return true;
        }

        public virtual bool Delete(T entity)
        {
            db.Entry(entity).State = EntityState.Deleted;

            //return db.SaveChanges() > 0;

            return true;

        }

        public virtual int Delete(params int[] ids)
        {
            //T entity =new T();
            //entity.ID
            //首先可以通过  泛型的基类的约束来实现对id字段赋值。
            //也可也使用反射的方式。
            foreach (var item in ids)
            {
                var entity = db.Set<T>().Find(item);//如果实体已经在内存中，那么就直接从内存拿，如果内存中跟踪实体没有，那么才查询数据库。
                db.Set<T>().Remove(entity);
            }



            //return db.SaveChanges();

            return ids.Count();

        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }

        public IQueryable<T> LoadPageEntities<S>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orderbyLambda, bool isAsc)
        {
            total = db.Set<T>().Where(whereLambda).Count();

            if (isAsc)
            {
                return
                db.Set<T>()
                  .Where(whereLambda)
                  .OrderBy(orderbyLambda)
                  .Skip(pageSize * (pageIndex - 1))
                  .Take(pageSize)
                  .AsQueryable();
            }
            else
            {
                return
               db.Set<T>()
                 .Where(whereLambda)
                 .OrderByDescending(orderbyLambda)
                 .Skip(pageSize * (pageIndex - 1))
                 .Take(pageSize)
                 .AsQueryable();
            }
        }
    }
}
