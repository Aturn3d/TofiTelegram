﻿using System;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Services.States.Commands;
using Bot.Services.States.InternetPayment;
using Bot.Services.States.MobilePayment;
using Bot.Services.States.MoneyTransfer;
using Bot.Services.States.PurchasePayment;
using Bot.Services.States.TicketsPayment;
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

        protected async Task AskForCreditCard()
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
                case StatesTypes.InternetPaymentRequestDate:
                    return new InternetPaymentRequestDataState(botService, update);
                case StatesTypes.InternetPaymentConfirm:
                    return new InternetPaymentConfirmState(botService, update);
                case StatesTypes.MobilePaymentProvider:
                    return new MobilePaymentProviderState(botService, update);
                case StatesTypes.MobilePayment:
                    return new MobilePaymentState(botService, update);
                case StatesTypes.MobilePaymentRequestDate:
                    return new MobilePaymentRequestDataState(botService, update);
                case StatesTypes.MobilePaymentConfirm:
                    return new MobilePaymentConfirmState(botService, update);
                case StatesTypes.TicketsPayment:
                    return new TicketsPaymentState(botService, update);
                case StatesTypes.TicketsPaymentRequestDate:
                    return new TicketsPaymentRequestDataState(botService, update);
                case StatesTypes.TicketsPaymentConfirm:
                    return new TicketsPaymentConfirmState(botService, update);
                case StatesTypes.PurchasePayment:
                    return new PurchasePaymentState(botService, update);
                case StatesTypes.PurchasePaymentRequestDate:
                    return new PurchasePaymentRequestDataState(botService, update);
                case StatesTypes.PurchasePaymentConfirm:
                    return new PurchasePaymentConfirmState(botService, update);
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
        InternetPayment,
        InternetPaymentRequestDate,
        InternetPaymentConfirm,
        MobilePaymentProvider,
        MobilePayment,
        MobilePaymentRequestDate,
        MobilePaymentConfirm,
        TicketsPayment,
        TicketsPaymentRequestDate,
        TicketsPaymentConfirm,
        PurchasePayment,
        PurchasePaymentRequestDate,
        PurchasePaymentConfirm,
    }
}
