using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.MobilePayment
{
    internal class MobilePaymentProviderState: PaymentsStateBase
    {
      

        public MobilePaymentProviderState(TelegramBotService botService, Update update) : base(botService, update) {}
        internal override StatesTypes StateTypesId => StatesTypes.MobilePaymentProvider;

        protected override async Task HandlePayment()
        {
            BotService.SetState(new MobilePaymentState(BotService, Update));
            await TaskCompleted;
        }

        public override async Task PrepareState()
        {
            await AskForCreditCard();
        }
    }

    enum MobileProviders
    {
      Mts,
      Velcome,
      Life
    }

}
