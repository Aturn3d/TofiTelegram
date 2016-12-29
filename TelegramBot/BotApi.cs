using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = Bot.Model.User;

namespace TelegramBot
{
    public class BotApi
    {
        private static readonly Lazy<TelegramBotClient> Lazy =
            new Lazy<TelegramBotClient>(() => new TelegramBotClient(ConfigurationManager.AppSettings["Token"]));
        public static TelegramBotClient Bot => Lazy.Value;


        static BotApi()
        {
           // Юзать при деплое(С Webhooks)
          //  Bot.SetWebhookAsync("https://tofibot.azurewebsites.net/api/Home");
        }

        public static async Task<User> GetMe()
        {
            Bot.StartReceiving();
            var t = await Bot.GetMeAsync();
            return new User
            {
                NickName = t.Username
            };
        }

        public static async Task Send(long chatId, string text)
        {
            await Bot.SendTextMessageAsync(chatId, text);
        }
        
    }
}
