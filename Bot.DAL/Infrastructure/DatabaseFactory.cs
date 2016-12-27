namespace Bot.DAL.Infrastructure
{
    public class DatabaseFactory : Disposable,IDatabaseFactory
    {

        private BotEntities context;

        protected override void DisposeCore()
        {
            if (context != null)
                context.Dispose();
        }

        public BotEntities Context
        {
            get
            {
                return context ?? (context = new BotEntities());
            }
        }
    }
}
