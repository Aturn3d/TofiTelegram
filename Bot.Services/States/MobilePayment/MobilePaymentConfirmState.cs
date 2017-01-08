using System;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.MobilePayment
{
   internal class MobilePaymentConfirmState : ConfirmStateBase
    {
       public MobilePaymentConfirmState(TelegramBotService botService, Update update) : base(botService, update) {}
       internal override StatesTypes StateTypesId => StatesTypes.MobilePaymentConfirm;
       protected override async Task HandlePayment()
       {
            var mess = Update.Message;
            if (mess?.Text == null)
                await HandleError();
            else
            {
                var response = BotService.PaymentService.PhonePay(BotService.User);
                await
                    BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, response.Description,
                        replyMarkup: new ReplyKeyboardHide());
                AddPayment(response);
                BotService.SetState(new InitialState(BotService, Update));
            }
        }

       protected override string PaymentDetails => $"Operator: {BotService.User.CurrentPayment.To}\nYour mobile number: {BotService.User.CurrentPayment.Account}";
       protected override void SetPreviousState()
       {
         BotService.SetState(new MobilePaymentProviderState(BotService, Update));
       }
    }
}
