using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Bot.DAL.Infrastructure;
using Bot.Model;

namespace Bot.DAL.Repositories
{
    public class UserRepository:RepositoryBase<User>,IUserRepository
    {
        public UserRepository(IDatabaseFactory dbF) : base(dbF) { }

        public override User Get(Expression<Func<User, bool>> where)
        {
               return dbSet.Where(where)
              .Include(u => u.CreditCard)
              .Include(u => u.Payments)
              .FirstOrDefault();
        }
    }

    public interface IUserRepository : IRepository<User> { }   
}
