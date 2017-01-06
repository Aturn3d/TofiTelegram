using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.Commands
{
    internal class PaymentCommand:ICommand
    {
        private static Lazy<InlineKeyboardMarkup> keyboard = new Lazy<InlineKeyboardMarkup>(
            () => new InlineKeyboardMarkup(new[]
               {
                    new[] // first row
                    {
                        new InlineKeyboardButton("Внести депозит", ((int)Payments.Deposit).ToString()),
                    
                    },
                    new[] // second row
                    {
                        new InlineKeyboardButton("Money Transfer",  ((int)Payments.MoneyTransfer).ToString()),
                      //  new InlineKeyboardButton("2.2")
                    }
                }));

        public async Task Execute(TelegramBotService botService)
        {
            await botService.Bot.SendTextMessageAsync(botService.User.ChatId, "Choose payment",
                replyMarkup: keyboard.Value);
            botService.SetState(new PaymentStartState(botService, null));
        }
    }
}
