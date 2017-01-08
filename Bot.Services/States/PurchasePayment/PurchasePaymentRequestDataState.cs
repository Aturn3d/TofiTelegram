using System.Threading.Tasks;
using Bot.Model;
using Bot.Model.ModelsForPayment;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Bot.Services.States.MoneyTransfer;
using Bot.Services.States.PurchasePayment;
using Telegram.Bot.Types;

namespace Bot.Services.States.TicketsPayment
{
    internal class PurchasePaymentRequestDataState : State
    {
        public PurchasePaymentRequestDataState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var mess = Update.Message;
            if (mess?.Text == null) {
                await HandleError();
            }
            else {
                var code = mess.Text.Trim();
                var purchase = Purchase.GetPurchase(code);
                if (purchase != null) {
                    var pay = new CurrentPaymentInfo
                    {
                        To = purchase.ToString(),
                        Account = code,
                        Amount = purchase.Amount
                    };
                    BotService.User.CurrentPayment = pay;
                    BotService.SetState(new PurchasePaymentConfirmState(BotService, Update));

                } else {
                    await HandleError();
                }
            }
        }

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Enter code of your purchase request\nFor Test(length of code should be equal to 4)");
        }

        protected override async Task HandleError()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Your code is not valid");
        }

        internal override StatesTypes StateTypesId => StatesTypes.PurchasePaymentRequestDate;
    }
}
