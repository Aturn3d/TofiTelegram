using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Bot.Model;

namespace Bot.DAL
{
    public class BotEntities : DbContext
    {
        public BotEntities()
#if DEBUG
            : base("BotConnectionTest")
#else 
           :base("Azure")
#endif
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BotEntities>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            
            base.OnModelCreating(modelBuilder);
            //Коли будут проблемы - раскоментить
        //    modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }


        public virtual void Commint()
        {
            base.SaveChanges();
        }

    }
}
