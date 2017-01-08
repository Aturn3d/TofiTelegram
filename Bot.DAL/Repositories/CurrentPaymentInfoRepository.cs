using Bot.DAL.Infrastructure;
using Bot.Model;

namespace Bot.DAL.Repositories
{
    public class CurrentPaymentInfoRepository:RepositoryBase<CurrentPaymentInfo> , ICurrentPaymentInfoRepository
    {
        public CurrentPaymentInfoRepository(IDatabaseFactory dbF) : base(dbF) {}
    
    }

    public interface ICurrentPaymentInfoRepository : IRepository<CurrentPaymentInfo> {}
}

