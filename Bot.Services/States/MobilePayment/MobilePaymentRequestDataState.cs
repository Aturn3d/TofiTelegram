using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.MobilePayment
{
    internal class MobilePaymentRequestDataState : State
    {
        private string _acountNumber;
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
                    //  var inter = new Internet()
                    //var pay = new CurrentPaymentInfo
                    //{
                    //    Amount = _amount,
                    //    To = _cardNum
                    //};
                    //BotService.User.CurrentPayment = pay;
                    //BotService.SetState(new MoneyTransferConfirmState(BotService, Update));
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
