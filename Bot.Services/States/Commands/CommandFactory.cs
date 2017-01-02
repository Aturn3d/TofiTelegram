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
            switch (text.ToUpperInvariant()) {
                case "/EXCHANGE":
                    return new ExchangeCommand();
                case "/RETURN":
                    return new ReturnToMainMenuCommand();
                case "/HELP":
                    return new HelpCommand();
                case "/PAYMENT":
                    return new PaymentCommand();
                default:
                    return null;
            }
        }
    }
}
