using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.MoneyTransfer
{
    internal class MoneyTransferConfirmState:ConfirmStateBase
    {
        public MoneyTransferConfirmState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.MoneyTransferConfirm;

        protected override async Task HandlePayment()
        {
            var mess = Update.Message;
            if (mess?.Text == null)
                await HandleError();
            else {
                var response = BotService.PaymentService.TransferMoney(BotService.User);
                await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, response.Description, replyMarkup: new ReplyKeyboardHide());
                AddPayment(response);
                await BotService.SetState(new InitialState(BotService, Update));
            }
           
        }

        protected override string PaymentDetails => $"Recipient's card number:{BotService.User.CurrentPayment.To}";

        protected override async Task SetPreviousState()
        {
            await AskForCreditCard(new MoneyTransferState(BotService, Update));
        }
    }
}
