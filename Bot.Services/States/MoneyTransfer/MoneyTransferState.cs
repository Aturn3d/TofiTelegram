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
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Enter recipient's card number and amount of transfer money delemited by spase");
            await BotService.SetState(new MoneyTransferRequestDataState(BotService, Update));
        }

        internal override StatesTypes StateTypesId => StatesTypes.MoneyTransfer;
    }
}
