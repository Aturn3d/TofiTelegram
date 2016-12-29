using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Bot.Services
{
    public interface IBotFactory
    {
        ITelegramBotClient GetTelegramBot();
    }

    public class BotFactory: IBotFactory
    {
        public Lazy<string> Token = new Lazy<string>(() => ConfigurationManager.AppSettings["Token"]);
        public ITelegramBotClient GetTelegramBot()
        {
          return new TelegramBotClient(Token.Value);
        }
    }
}
