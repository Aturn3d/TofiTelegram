using System;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Telegram.Bot.Types;

namespace Bot.Services.States.MobilePayment
{
   internal class MobilePaymentsConfirmState : ConfirmStateBase
    {
       public MobilePaymentsConfirmState(TelegramBotService botService, Update update) : base(botService, update) {}
       internal override StatesTypes StateTypesId { get; }
       protected override Task HandlePayment()
       {
           throw new NotImplementedException();
       }

       protected override string PaymentDetails { get; }
       protected override async Task SetPreviousState()
       {
         await BotService.SetState(new InternetPaymentProviderState(BotService, Update));
       }
    }
}
