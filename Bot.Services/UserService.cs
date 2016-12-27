using System.Collections.Generic;
using Bot.DAL.Infrastructure;
using Bot.DAL.Repositories;
using Bot.Model;

namespace Bot.Services
{
    public interface IUserService
    {
        User GetUser(int id);
        User GetUserByName(string name);
        IEnumerable<User> GetUsers();
        void Update(User user);
        bool CreateUser(User user);
        IEnumerable<User> Search(string search);
    }


    public class UserService : IUserService
    {
        private IUnitOfWork uof;
        private IUserRepository userRepository;

        public UserService(IUnitOfWork uof, IUserRepository userRepository)
        {
            this.uof = uof;
            this.userRepository = userRepository;
        }

        public User GetUser(int id)
        {
            return userRepository.GetById(id);
        }

        public bool CreateUser(User user)
        {
            if (user.UserId != 0) return false;
            userRepository.Add(user);
            uof.Commit();
            return true;
        }



        public User GetUserByName(string name)
        {
            return userRepository.Get(u => u.NickName.Equals(name));
        }

        public void Save()
        {
            uof.Commit();
        }

        public void Update(User user)
        {
            userRepository.Update(user);
            uof.Commit();
        }

        public IEnumerable<User> Search(string search)
        {
            var s = search.ToLower();
            return userRepository.GetMany(u => u.NickName.ToLower().Contains(s));
        }
       

        public IEnumerable<User> GetUsers()
        {
            return userRepository.GetAll();
        }
        
    }
}
