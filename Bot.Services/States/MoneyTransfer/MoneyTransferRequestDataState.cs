using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.Common;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.MoneyTransfer
{
    internal class MoneyTransferRequestDataState : State
    {
        private decimal _amount;
        private string _cardNum;
        public MoneyTransferRequestDataState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var mess = Update.Message;
            if (mess?.Text == null) {
                await HandleError();
            }
            else {
                if (Validate(mess.Text)) {
                    var pay = new CurrentPaymentInfo
                    {
                        Amount = _amount,
                        To = _cardNum
                    };
                    BotService.User.CurrentPayment = pay;
                    BotService.SetState(new MoneyTransferConfirmState(BotService, Update));
                }
                else {
                    await HandleError();
                }
            }
        }

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Enter recipient's card number and amount of transfer money delemited by spase");
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


        internal override StatesTypes StateTypesId => StatesTypes.MoneyTransferRequestDate;
    }
}
