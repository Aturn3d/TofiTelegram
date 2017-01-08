using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.InternetPayment
{
    internal class InternetPaymentProviderState: PaymentsStateBase
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



        public InternetPaymentProviderState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.InternetPaymentProvider;

        protected override async Task HandlePayment()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Choose your Internet provider", replyMarkup: keyboard);
            BotService.SetState(new InternetPaymentState(BotService, Update));
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
