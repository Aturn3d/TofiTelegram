using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Bot.DAL.Infrastructure;
using Bot.DAL.Repositories;
using Bot.Services;

namespace Bot.Web
{
    public static class DIConfig
    {

        public static void Config(HttpConfiguration config)
        {

            var builder = new ContainerBuilder();    
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            IContainer container =  builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }




    }
}