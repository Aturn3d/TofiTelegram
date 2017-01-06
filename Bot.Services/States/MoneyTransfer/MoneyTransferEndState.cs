using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.MoneyTransfer
{
    internal class MoneyTransferEndState:State
    {
        decimal _amount;
        string _cardNum;
        public MoneyTransferEndState(TelegramBotService botService, Update update) : base(botService, update) {}
        protected override async Task Handle()
        {
            var mess = Update.Message;
            if (mess?.Text == null)
                await HandleError();
            else {
                if (Validate(mess.Text)) {
                   var response =  BotService.PaymentService.TransferMoney(BotService.User, _cardNum, _amount);
                   await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, response.Description);
                   AddPayment(response);
                   BotService.SetState(new InitialState(BotService, Update));
                }
            }
        }

        private bool Validate(string text)
        {
            var splited = text.Split(' ');
            var bo = splited.Length == 2 && Validators.ValidateCardNumber(splited[0]) &&
                   decimal.TryParse(splited[1], out _amount)
                   && _amount > 0;
            if (bo) {
                _cardNum = splited[0];
            }
            return bo;
        }


        internal override StatesTypes StateTypesId => StatesTypes.MoneyTransferEnd;
    }
}
