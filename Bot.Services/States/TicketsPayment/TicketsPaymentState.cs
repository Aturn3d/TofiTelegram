using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;

namespace Bot.Services.States.TicketsPayment
{
    internal class TicketsPaymentState: PaymentsStateBase
    {
        public TicketsPaymentState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task HandlePayment()
        {
            BotService.SetState(new TicketsPaymentRequestDataState(BotService, Update));
            await TaskCompleted;
        }

        internal override StatesTypes StateTypesId => StatesTypes.TicketsPayment;
    }
}
