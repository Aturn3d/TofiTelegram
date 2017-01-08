using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentConfirmState : ConfirmStateBase
    {
        public InternetPaymentConfirmState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.InternetPaymentConfirm;

        protected override async Task HandlePayment()
        {
            var mess = Update.Message;
            if (mess?.Text == null) {
                await HandleError();
            }
            else {
                var response = BotService.PaymentService.InternetPay(BotService.User);
                await
                    BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, response.Description,
                        replyMarkup: new ReplyKeyboardHide());
                AddPayment(response);
                BotService.SetState(new InitialState(BotService, Update));
            }
        }

        protected override string PaymentDetails
            =>
                $"Your Porvider:{BotService.User.CurrentPayment.To}\n Your Account:{BotService.User.CurrentPayment.Account}"
            ;

        protected override void SetPreviousState()
        {
            BotService.SetState(new InternetPaymentProviderState(BotService, Update));
        }
    }
}
