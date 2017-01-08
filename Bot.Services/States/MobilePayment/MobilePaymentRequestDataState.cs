using System;
using System.Threading.Tasks;
using Bot.Model.ModelsForPayment;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.MobilePayment
{
    internal class MobilePaymentRequestDataState : State
    {
        private string _phoneNumber;
        private decimal _amount;
        public MobilePaymentRequestDataState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var mess = Update.Message;
            if (mess?.Text == null) {
                await HandleError();
            }
            else {
                if (Parse(mess.Text)) {
                    if (Parse(mess.Text)) {
                        var phone = new Phone(BotService.User.CurrentPayment.To, _phoneNumber, _amount);
                        var errors = Validators.ValidatePhone(phone);
                        if (errors.Count == 0) {
                            var pay = BotService.User.CurrentPayment;
                            pay.Account = _phoneNumber;
                            pay.Amount = _amount;
                            BotService.User.CurrentPayment = pay;
                            await BotService.SetState(new MobilePaymentConfirmState(BotService, Update));
                        }
                        else {
                            var errorMessage = "Your input is not valid. Correct issues below and continue\n" +
                                               string.Join(Environment.NewLine, errors);
                            ;
                            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, errorMessage);
                        }
                    }
                    else {
                        await HandleError();
                    }
                }
            }
        }

        private bool Parse(string text)
        {
            var splited = text.Split(' ');
            var bo = splited.Length == 2 &&
                     decimal.TryParse(splited[1], out _amount);
            if (bo) {
                _phoneNumber = splited[0];
            }
            return bo;
        }


        internal override StatesTypes StateTypesId => StatesTypes.MobilePaymentRequestDate;
    }
}
