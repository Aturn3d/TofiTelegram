using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.TicketsPayment;
using Telegram.Bot.Types;

namespace Bot.Services.States.PurchasePayment
{
    internal class PurchasePaymentState: PaymentsStateBase
    {
        public PurchasePaymentState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task HandlePayment()
        {
            BotService.SetState(new PurchasePaymentRequestDataState(BotService, Update));
            await TaskCompleted;
        }

        internal override StatesTypes StateTypesId => StatesTypes.PurchasePayment;
    }
}
