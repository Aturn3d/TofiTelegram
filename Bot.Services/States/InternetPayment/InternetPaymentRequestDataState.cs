using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentRequestDataState:State
    {
        public InternetPaymentRequestDataState(TelegramBotService botService, Update update) : base(botService, update) {}
        protected override Task Handle()
        {
            throw new NotImplementedException();
        }

        internal override StatesTypes StateTypesId { get; }
    }
}
