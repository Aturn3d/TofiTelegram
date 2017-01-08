using System.Threading.Tasks;
using Bot.Model;
using Bot.Model.ModelsForPayment;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;

namespace Bot.Services.States.TicketsPayment
{
    internal class TicketsPaymentRequestDataState : State
    {
        public TicketsPaymentRequestDataState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var mess = Update.Message;
            if (mess?.Text == null) {
                await HandleError();
            }
            else {
                var code = mess.Text.Trim();
                var ticket = Ticket.GetTicket(code);
                if (ticket != null) {
                    var pay = new CurrentPaymentInfo
                    {
                        To = ticket.ToString(),
                        Account = code,
                        Amount = ticket.Amount
                    };
                    BotService.User.CurrentPayment = pay;
                    BotService.SetState(new TicketsPaymentConfirmState(BotService, Update));

                } else {
                    await HandleError();
                }
            }
        }

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Enter code of your ticket payment request");
        }

        protected override async Task HandleError()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Your code is not valid");
        }

        internal override StatesTypes StateTypesId => StatesTypes.TicketsPaymentRequestDate;
    }
}
