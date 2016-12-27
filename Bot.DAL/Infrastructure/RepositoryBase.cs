using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Bot.DAL.Infrastructure
{
    public class RepositoryBase<T> : IRepository<T> where T :class
    {
        private BotEntities dataContext;
        protected IDbSet<T> dbSet;


        public RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbSet = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory { get; }

        protected BotEntities DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Context); }
        }
        public virtual void Add(T entity) => dbSet.Add(entity);


        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var entities = dbSet.Where(where).AsEnumerable();
            foreach (var e in entities)
                dbSet.Remove(e);
        }


        public virtual void Delete(T entity) => dbSet.Remove(entity);

        public virtual int Count() => dbSet.Count();

        public virtual int Count(Expression<Func<T, bool>> where) => dbSet.Count(where);

        public virtual T Get(Expression<Func<T, bool>> where) => dbSet.Where(where).FirstOrDefault();


        public virtual IEnumerable<T> GetAll() => dbSet.ToList();


        public virtual T GetById(string id) => dbSet.Find(id);


        public virtual T GetById(long id) => dbSet.Find(id);


        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where) => dbSet.Where(where).ToList();
       
        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
