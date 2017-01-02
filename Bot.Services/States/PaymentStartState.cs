using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States
{
    internal class PaymentStartState:State
    {
        public PaymentStartState(TelegramBotService botService, Update update) : base(botService, update) {}
        protected override async Task Handle()
        {
            var query = Update.CallbackQuery;
            if (query != null) {
                await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, query.Data);
                await BotService.Bot.AnswerCallbackQueryAsync(query.Id);
            }
            else {
                await HandleError();
            }
           
        }

        public override StatesTypes StateTypesId => StatesTypes.PayStart;
    }

    enum Payments
    {
        Deposit,
        MoneyTransfer
    }
}
