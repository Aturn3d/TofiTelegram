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
        public static Lazy<string> Token = new Lazy<string>(() => ConfigurationManager.AppSettings["Token"]);
        private static Lazy<TelegramBotClient> Bot = new Lazy<TelegramBotClient>(
            () =>
            {
                var bot = new TelegramBotClient(Token.Value);
#if !DEBUG
                bot.SetWebhookAsync("https://tofibot.azurewebsites.net/api/Home");
#endif
                return bot;
            });
        public ITelegramBotClient GetTelegramBot()
        {
          return Bot.Value;
        }
    }
}
