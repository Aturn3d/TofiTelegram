using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Bot.Model;

namespace Bot.DAL
{
    public class BotEntities : DbContext
    {

        public BotEntities() : base("BotConnectionTest")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BotEntities>());
        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<User>().HasRequired(u => u.PasswordHash);

            
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
