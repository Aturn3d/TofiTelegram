using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.States.Commands
{
    internal interface ICommand
    {
        Task Execute(TelegramBotService botService);
    }

    internal static class CommandFactory
    {
        public static ICommand CreateCommand(string text)
        {
            //TODO: Сделать свитч?
            if (text.ToUpperInvariant().StartsWith("/EXCHANGE")) {
                return new ExchangeCommand();
            }
            return null;
        }
    }
}
