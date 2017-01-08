using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.States.Commands;
using Bot.Services.States.InternetPayment;
using Bot.Services.States.MoneyTransfer;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.Base
{
    public abstract class State
    {
        protected const string CardItemsDelimeter = "|";
        // await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        private static Lazy<InlineKeyboardMarkup> keyboardYesNo = new Lazy<InlineKeyboardMarkup>(
            () => new InlineKeyboardMarkup(new[]
               {
                    new[] // first row
                    {
                        new InlineKeyboardButton("Yes", "1"),
                        new InlineKeyboardButton("No", "0"),
                    }
                }));


        public static readonly Task TaskCompleted = Task.FromResult(0);
        protected Update Update;
        protected TelegramBotService BotService;

        internal State(TelegramBotService botService, Update update)
        {
            BotService = botService;
            Update = update;
        }

        protected abstract Task Handle();
        internal abstract StatesTypes StateTypesId { get; }

        public async Task HandleUpdate()
        {
            var handled = await HandleCommands().ConfigureAwait(false);
            if (!handled) {
                await Handle();
            }
        }

        public virtual Task PrepareState()
        {
            //Basic behavior. Do nothing
            return TaskCompleted;
        }

        protected void AddPayment(Payment p)
        {
            BotService.User.Payments.Add(p);
        }

        protected virtual async Task HandleError()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Your input is incorrect! No soup for you");
        }

        protected async Task AskForCreditCard(State nextState)
        {
            const string text = "Please, enter your credit card data in such format: card_number" + CardItemsDelimeter + "cvv_code" + CardItemsDelimeter + "expiration_date." +
                                "For test purpose use this data: 4111111111111111"+ CardItemsDelimeter + "123" + CardItemsDelimeter + "1217";
            if (BotService.User.CreditCard != null) {
                var card = BotService.User.CreditCard.CardNumber;
                await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId,
                    $"Use existin card?{Environment.NewLine} Card Number:{card.Remove(0, card.Length - 4).PadLeft(card.Length, '*')}",
                    replyMarkup: keyboardYesNo.Value);
            }
            else {
                await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, text);
            }
            BotService.SetState(nextState);
        }

        private async Task<bool> HandleCommands()
        {
            var message = Update.Message;
            if (message == null) {
                return false;
            }
            var command = CommandFactory.CreateCommand(message.Text);
            if (command == null) {
                return false;
            }
            await command.Execute(BotService);
            return true;
        }

        #region FactoryMethod
        internal static State GetState(StatesTypes stateTypesId, TelegramBotService botService, Update update)
        {
            switch (stateTypesId)
            {
                case StatesTypes.InitialState:
                    return new InitialState(botService, update);
                case StatesTypes.Exchange:
                    return new ExchangeState(botService, update);
                case StatesTypes.PayStart:
                    return new PaymentStartState(botService, update);
                case StatesTypes.MoneyTransfer:
                    return new MoneyTransferState(botService, update);
                case StatesTypes.MoneyTransferRequestDate:
                    return new MoneyTransferRequestDataState(botService, update);
                case StatesTypes.MoneyTransferConfirm:
                    return new MoneyTransferConfirmState(botService, update);
                case StatesTypes.InternetPaymentProvider:
                    return new InternetPaymentProviderState(botService, update);
                case StatesTypes.InternetPayment:
                    return new InternetPaymentState(botService, update);
                default:
                    //return GetDefaultState(botService, update);
                    throw new NotSupportedException();
            }
        }

        public static State GetDefaultState(TelegramBotService botService, Update update) => new InitialState(botService, update);
        #endregion
    }

    internal enum StatesTypes
    {
        InitialState = 0,
        Exchange,
        PayStart,
        MoneyTransfer,
        MoneyTransferRequestDate,
        MoneyTransferConfirm,
        InternetPaymentProvider,
        InternetPayment
    }
}
