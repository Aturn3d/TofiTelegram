using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Model.ModelsForPayment;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentRequestDataState : State
    {
        private string _acountNumber;
        private decimal _amount;
        public InternetPaymentRequestDataState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var mess = Update.Message;
            if (mess?.Text == null) {
                await HandleError();
            }
            else {
                if (Parse(mess.Text)) {
                    var inter = new Internet(BotService.User.CurrentPayment.To, _acountNumber, _amount);
                    var errors = Validators.ValidateInternet(inter);
                    if (errors.Count == 0) {
                        var pay = BotService.User.CurrentPayment;
                        pay.Account = _acountNumber;
                        pay.Amount = _amount;
                        BotService.User.CurrentPayment = pay;
                        await BotService.SetState(new InternetPaymentConfirmState(BotService, Update));
                    } else {
                        var errorMessage = "Your input is not valid. Correct issues below and continue\n" +
                             string.Join(Environment.NewLine, errors); ;
                        await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, errorMessage);
                    }
                }
                else {
                    await HandleError();
                }
            }
        }

        private bool Parse(string text)
        {
            var splited = text.Split(' ');
            var bo = splited.Length == 2 &&
                     decimal.TryParse(splited[1], out _amount);
            if (bo) {
                _acountNumber = splited[0];
            }
            return bo;
        }


        internal override StatesTypes StateTypesId => StatesTypes.InternetPaymentRequestDate;
    }
}
