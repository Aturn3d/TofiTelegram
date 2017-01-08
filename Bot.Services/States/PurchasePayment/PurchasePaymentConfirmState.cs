using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.PurchasePayment
{
    internal class PurchasePaymentConfirmState : ConfirmStateBase
    {
        public PurchasePaymentConfirmState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.TicketsPaymentConfirm;

        protected override async Task HandlePayment()
        {
            var mess = Update.Message;
            if (mess?.Text == null)
                await HandleError();
            else {
                var response = BotService.PaymentService.TransferMoney(BotService.User);
                await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, response.Description, replyMarkup: new ReplyKeyboardHide());
                AddPayment(response);
                BotService.SetState(new InitialState(BotService, Update));
            }
           
        }

        protected override string PaymentDetails => $"{BotService.User.CurrentPayment.To}";

        protected override void SetPreviousState()
        {
            BotService.SetState(new MoneyTransferState(BotService, Update));
        }
    }
}
