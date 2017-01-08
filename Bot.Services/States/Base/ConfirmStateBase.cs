using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.Base
{
   internal abstract class ConfirmStateBase:State
   {
       private const string Confirm = "Confirm";
       private const string Cancel = "Cancel";

        private static ReplyKeyboardMarkup confirmKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton(Confirm),
                    new KeyboardButton(Cancel)
                });

       protected ConfirmStateBase(TelegramBotService botService, Update update) : base(botService, update) {}

        protected sealed override async Task Handle()
        {
            var mes = Update.Message;
            if (mes == null) {
                await HandleError();
                return;
            }

            var text = mes.Text;
            switch (text) {
                case Confirm:
                    await HandlePayment();
                    break;
                case Cancel:
                    SetPreviousState();
                    break;
                default:
                    await HandleError();
                    break;
            }

        }

       public override async Task PrepareState()
       {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, 
                $"Please, confirm yor payment\nCard:{BotService.User.CreditCard.CardNumber}\n" + PaymentDetails + $"\nTotal Amount: {BotService.User.CurrentPayment.Amount.ToString()}", 
                replyMarkup: confirmKeyboard);
        }

       protected abstract Task HandlePayment();
       protected abstract string PaymentDetails { get; }
       protected abstract void SetPreviousState();
    }
}
