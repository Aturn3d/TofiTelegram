using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Bot.Services.States.InternetPayment;
using Bot.Services.States.MobilePayment;
using Bot.Services.States.MoneyTransfer;
using Bot.Services.States.PurchasePayment;
using Bot.Services.States.TicketsPayment;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States
{
    internal class PaymentStartState:State
    {

        private static Lazy<InlineKeyboardMarkup> keyboard = new Lazy<InlineKeyboardMarkup>(
            () => new InlineKeyboardMarkup(new[]
               {
                    //new[] // first row
                    //{
                    //    new InlineKeyboardButton("Внести депозит", ((int)Payments.Deposit).ToString()),

                    //},
                    new[] // second row
                    {
                        new InlineKeyboardButton("Money Transfer",  ((int)Payments.MoneyTransfer).ToString()),
                      //  new InlineKeyboardButton("2.2")
                    },
                      new[]
                    {
                        new InlineKeyboardButton("Pay for internet",  ((int)Payments.InternetPayment).ToString()),

                    },
                      new[]
                    {
                        new InlineKeyboardButton("Pay for mobile",  ((int)Payments.MobilePayment).ToString()),

                    },
                      new[]
                    {
                        new InlineKeyboardButton("Pay for ticket",  ((int)Payments.TicketsPayment).ToString()),

                    },
                       new[]
                    {
                        new InlineKeyboardButton("Pay for purchase",  ((int)Payments.PurchasePayment).ToString()),

                    }
                }));
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
                        BotService.SetState(new MoneyTransferState(BotService, Update));
                        break;
                    case Payments.InternetPayment:
                        BotService.SetState(new InternetPaymentProviderState(BotService, Update));
                        break;
                    case Payments.MobilePayment:
                        BotService.SetState(new MobilePaymentProviderState(BotService, Update));
                        break;
                    case Payments.TicketsPayment:
                        BotService.SetState(new TicketsPaymentState(BotService, Update));
                        break;
                    case Payments.PurchasePayment:
                        BotService.SetState(new PurchasePaymentState(BotService, Update));
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

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Choose payment",
                replyMarkup: keyboard.Value);
        }
    }

    enum Payments
    {
        Deposit,
        MoneyTransfer,
        InternetPayment,
        MobilePayment,
        TicketsPayment,
        PurchasePayment
    }
}
