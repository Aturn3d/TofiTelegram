using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.InternetPayment
{
   internal class InternetPaymentsConfirmState: ConfirmStateBase
    {
       public InternetPaymentsConfirmState(TelegramBotService botService, Update update) : base(botService, update) {}
       internal override StatesTypes StateTypesId => StatesTypes.InternetPaymentConfirm;
       protected override Task HandlePayment()
       {
           throw new NotImplementedException();
       }

       protected override string PaymentDetails => $"Your Porvider:{BotService.User.CurrentPayment.To}\n Your Account:{BotService.User.CurrentPayment.Account}";
        protected override async Task SetPreviousState()
       {
         await BotService.SetState(new InternetPaymentProviderState(BotService, Update));
       }
    }
}
