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
                        new InlineKeyboardButton("Внести депозит", nameof(Payments.Deposit)),
                    
                    },
                    new[] // second row
                    {
                        new InlineKeyboardButton("Перевод денег со счета на счет", nameof(Payments.MoneyTransfer)),
                      //  new InlineKeyboardButton("2.2")
                    }
                }));

        public async Task Execute(TelegramBotService botService)
        {
            await botService.Bot.SendTextMessageAsync(botService.User.ChatId, "Возможные типы оплаты",
                replyMarkup: keyboard.Value);
            botService.SetState(new PaymentStartState(botService, null));
        }
    }
}
