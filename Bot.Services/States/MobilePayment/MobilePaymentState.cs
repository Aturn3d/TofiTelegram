using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.MobilePayment
{
    internal class MobilePaymentState : State
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
                if (Enum.IsDefined(typeof (MobileProviders), p)) {
                    var provider = ((MobileProviders) p).ToString();
                    BotService.User.CurrentPayment = new CurrentPaymentInfo
                    {
                        To = provider
                    };
                    BotService.SetState(new MobilePaymentRequestDataState(BotService, Update));
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
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Choose your mobile operator", replyMarkup: keyboard);
        }

        internal override StatesTypes StateTypesId => StatesTypes.MobilePayment;
    }
}
