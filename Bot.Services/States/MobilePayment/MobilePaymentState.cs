using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Telegram.Bot.Types;

namespace Bot.Services.States.MobilePayment
{
    internal class MobilePaymentState : State
    {
        public MobilePaymentState(TelegramBotService botService, Update update) : base(botService, update) {}

        protected override async Task Handle()
        {
            var query = Update.CallbackQuery;
            if (query != null) {
                int p;
                if (!int.TryParse(query.Data, out p)) {
                    await HandleError();
                    return;
                }
                if (Enum.IsDefined(typeof (InternetPayment.InternetProviders), p)) {
                    var provider = ((InternetPayment.InternetProviders) p).ToString();
                    BotService.User.CurrentPayment = new CurrentPaymentInfo
                    {
                        To = provider
                    };
                    await BotService.SetState(new MobilePaymentRequestDataState(BotService, Update));
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
