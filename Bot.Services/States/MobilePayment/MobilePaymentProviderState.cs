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
                        new InlineKeyboardButton("MTS", ((int) InternetProviders.ByFly).ToString()),
                        new InlineKeyboardButton("Life", ((int) InternetProviders.CosmosTv).ToString()),
                        new InlineKeyboardButton("Velcome", ((int) InternetProviders.CosmosTv).ToString())
                    }
                });



        public MobilePaymentProviderState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.MobilePaymentProvider;

        protected override async Task HandlePayment()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Choose your mobile operator", replyMarkup: keyboard);
            await BotService.SetState(new MobilePaymentState(BotService, Update));
        }
    }

    enum MobileProviders
    {
      Mts,
      Velcome,
      Life
    }

}
