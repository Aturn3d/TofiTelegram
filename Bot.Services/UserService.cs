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
        User GetByChatId(long chatId);
        User GetByTelegramUserId(int userId);
        IEnumerable<User> GetUsers();
        void Update(User user);
        bool CreateUser(User user);
        IEnumerable<User> Search(string search);
        void CreateOrUpdateUser(User user);
        void DeleteCurrentPayment(int userId);
    }


    public class UserService : IUserService
    {
        private IUnitOfWork uof;
        private IUserRepository userRepository;
        private ICurrentPaymentInfoRepository currentPaymentInfoRepository;

        public UserService(IUnitOfWork uof, IUserRepository userRepository, ICurrentPaymentInfoRepository currentPaymentInfoRepository)
        {
            this.uof = uof;
            this.userRepository = userRepository;
            this.currentPaymentInfoRepository = currentPaymentInfoRepository;
        }

        public User GetUser(int id)
        {
            return userRepository.GetById(id);
        }

        public bool CreateUser(User user)
        {
            if (user.Id != 0) return false;
            userRepository.Add(user);
            Save();
            return true;
        }



        public User GetUserByName(string name)
        {
            return userRepository.Get(u => u.NickName.Equals(name));
        }

        public User GetByChatId(long chatId)
        {
            return userRepository.Get(u => u.ChatId == chatId);
        }

        public User GetByTelegramUserId(int userId)
        {
            return userRepository.Get(u => u.UserId == userId);
        }

        private void Save()
        {
            uof.Commit();
        }

        public void Update(User user)
        {
            userRepository.Update(user);
            Save();
        }

        public IEnumerable<User> Search(string search)
        {
            var s = search.ToLower();
            return userRepository.GetMany(u => u.NickName.ToLower().Contains(s));
        }

        public void CreateOrUpdateUser(User user)
        {
            if (user.Id == 0) {
                CreateUser(user);
            }
            else {
                Update(user);
            }
        }

        public void DeleteCurrentPayment(int userId)
        {
            var user = GetUser(userId);
            if (user?.CurrentPayment != null) {
                currentPaymentInfoRepository.Delete(user.CurrentPayment);
            }
        }


        public IEnumerable<User> GetUsers()
        {
            return userRepository.GetAll();
        }
        
    }
}
