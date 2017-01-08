using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.States.Base;
using Telegram.Bot.Types;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentState : State
    {
        public InternetPaymentState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var query = Update.CallbackQuery;
            if (query != null) {
                int p;
                if (!int.TryParse(query.Data, out p)) {
                    await HandleError();
                    return;
                }
                if (Enum.IsDefined(typeof (InternetProviders), p)) {
                    var provider = ((InternetProviders) p).ToString();
                    BotService.User.CurrentPayment = new CurrentPaymentInfo
                    {
                        To = provider
                    };
                    BotService.SetState(new InternetPaymentRequestDataState(BotService, Update));
                }
                else {
                    await HandleError();
                }
            }
            else {
                await HandleError();
            }
        }

        internal override StatesTypes StateTypesId => StatesTypes.InternetPayment;
    }
}
