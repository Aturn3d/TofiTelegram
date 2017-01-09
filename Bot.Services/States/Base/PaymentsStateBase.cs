using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.Common;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.Base
{
    public abstract class PaymentsStateBase : State
    {
        private List<string> _errors;

        protected PaymentsStateBase(TelegramBotService botService, Update update) : base(botService, update) {}

        protected sealed override async Task Handle()
        {
            //Use existing card yes/no
            var query = Update.CallbackQuery;
            if (query != null) {
                await HandleQuery(query);
                return;
            }
            //User entered card number
            var mess = Update.Message;
            if (mess != null) {
                var card = ValidateInput(mess);
                if (card == null) {
                    var errorMessage = "You input is incorrect. Please correct errors below and try again" + Environment.NewLine +
                                       string.Join(Environment.NewLine, _errors);
                    await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, errorMessage, replyMarkup: new ReplyKeyboardHide());
                }
                else {
                    BotService.User.CreditCard = card;
                    BotService.UserService.DeleteCurrentPayment(BotService.User.Id);
                    await HandlePayment();
                }
            }
            else {
                await HandleError();
            }
        }

        public override async Task PrepareState()
        {
            await AskForCreditCard();
        }


        private async Task HandleQuery(CallbackQuery query)
        {
            var useCard = int.Parse(query.Data) > 0;
            if (useCard) {
                BotService.UserService.DeleteCurrentPayment(BotService.User.Id);
                await HandlePayment();
            }
            else {
                BotService.User.CreditCard = null;
            }
        }

        protected abstract Task HandlePayment();

        private CreditCard ValidateInput(Message message)
        {
            _errors = new List<string>(3);
            var items = message.Text.Split(CardItemsDelimeter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != 3) {
                _errors.Add("You dont enter all required data");
                return null;
            }
            if (!Validators.ValidateCardNumber(items[0])) {
                _errors.Add("Card number is not valid!");
            }
            if (!Validators.ValidateCvvCode(items[1])) {
                _errors.Add("Cvv is not valid!");
            }
            if (!Validators.IsExpirationCardDateValid(items[2])) {
                _errors.Add("Yor card is out of date");
            }
            if (_errors.Count > 0) {
                return null;
            }
            return new CreditCard
            {
                CardNumber = items[0],
                CvvCode = items[1],
                ExpirationDate = items[2]
            };
        }
    }
}
