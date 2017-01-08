using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Bot.DAL.Infrastructure;
using Bot.DAL.Repositories;
using Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Bot.ApplicationForTests
{
    class Program
    {
        static IContainer Container;
        static ITelegramBotClient Bot;
        static void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerLifetimeScope();
            builder.RegisterType<BotFactory>().As<IBotFactory>().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(TelegramBotService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            Container = builder.Build();
        }
        static void Main(string[] args)
        {
            ConfigureAutofac();
            IBotFactory fac = new BotFactory();
            Bot = fac.GetTelegramBot();
           
            Bot.OnUpdate += Bot_OnUpdate;
            Bot.OnReceiveError += BotOnReceiveError;
            var me = Bot.GetMeAsync().Result;


            Console.WriteLine(me.Username);
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }



        private static async void Bot_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<ITelegramBotService>();
                await service.HandleUpdate(e.Update);
            }
        }
        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Debugger.Break();
        }
    }
}
