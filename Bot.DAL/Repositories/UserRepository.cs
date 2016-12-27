using Bot.DAL.Infrastructure;
using Bot.Model;

namespace Bot.DAL.Repositories
{
    public class UserRepository:RepositoryBase<User>,IUserRepository
    {
        public UserRepository(IDatabaseFactory dbF) : base(dbF) { }
      
    }

    public interface IUserRepository : IRepository<User> { }   
}
