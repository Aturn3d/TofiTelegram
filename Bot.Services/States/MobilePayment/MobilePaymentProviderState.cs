using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.MobilePayment
{
    internal class MobilePaymentProviderState: PaymentsStateBase
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



        public MobilePaymentProviderState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.InternetPaymentProvider;

        protected override async Task HandlePayment()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Choose your Internet provider", replyMarkup: keyboard);
            await BotService.SetState(new MobilePaymentState(BotService, Update));
        }
    }

    enum InternetProviders
    {
        ByFly,
        CosmosTv,
        Adsl,
        AtlantTelecom
    }

}
