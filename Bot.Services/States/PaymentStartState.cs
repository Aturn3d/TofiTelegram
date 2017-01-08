using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;

namespace Bot.Services.States
{
    internal class PaymentStartState:State
    {
        public PaymentStartState(TelegramBotService botService, Update update) : base(botService, update) {}
        protected override async Task Handle()
        {
            var query = Update.CallbackQuery;
            if (query != null) {
                int p;
                if (!Int32.TryParse(query.Data, out p)) {
                    await HandleError();
                    return;
                }
                switch ((Payments) p) {
                    case Payments.Deposit:
                        break;
                    case Payments.MoneyTransfer:
                        await AskForCreditCard(new MoneyTransferState(BotService, Update));
                        break;
                    case Payments.InternetPayment:
                        await AskForCreditCard(new InternetPaymentProviderState(BotService, Update));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else {
                await HandleError();
            }
        }

        internal override StatesTypes StateTypesId => StatesTypes.PayStart;
    }

    enum Payments
    {
        Deposit,
        MoneyTransfer,
        InternetPayment
    }
}
