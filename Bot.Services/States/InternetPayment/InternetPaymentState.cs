using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.States.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentState : State
    {

        private InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        new InlineKeyboardButton("ByFly", ((int)InternetProviders.ByFly).ToString()),
                        new InlineKeyboardButton("CosmosTv",((int)InternetProviders.CosmosTv).ToString())
                    },
                    new[]
                    {
                        new InlineKeyboardButton("Adsl",((int)InternetProviders.Adsl).ToString()),
                        new InlineKeyboardButton("AtlantTelecom",((int)InternetProviders.AtlantTelecom).ToString())
                    }
                });

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

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Choose your Internet provider", replyMarkup: keyboard);
        }

        internal override StatesTypes StateTypesId => StatesTypes.InternetPayment;
    }
}
