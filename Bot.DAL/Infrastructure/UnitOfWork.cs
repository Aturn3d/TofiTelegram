namespace Bot.DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;
        private BotEntities dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected BotEntities DataContext => dataContext ?? (dataContext = databaseFactory.Context);

        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }
}
