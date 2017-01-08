using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.MoneyTransfer
{
    internal class MoneyTransferState: PaymentsStateBase
    {
        public MoneyTransferState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task HandlePayment()
        {
            BotService.SetState(new MoneyTransferRequestDataState(BotService, Update));
            await TaskCompleted;
        }

        internal override StatesTypes StateTypesId => StatesTypes.MoneyTransfer;
    }
}
