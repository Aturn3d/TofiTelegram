using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentProviderState: PaymentsStateBase
    {



        public InternetPaymentProviderState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.InternetPaymentProvider;

        protected override async Task HandlePayment()
        {
            BotService.SetState(new InternetPaymentState(BotService, Update));
            await TaskCompleted;
        }
    }

    enum InternetProviders
    {
        ByFly,
        CosmosTv,
        Adsl
    }

}
