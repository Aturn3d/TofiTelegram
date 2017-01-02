using System;
using System.Threading.Tasks;
using Bot.Services.States.Commands;
using Telegram.Bot.Types;

namespace Bot.Services.States.Base
{
    internal abstract class State
    {
        // await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

        public static readonly Task TaskCompleted = Task.FromResult(0);
        protected Update Update;
        protected TelegramBotService BotService;

        internal State(TelegramBotService botService, Update update)
        {
            BotService = botService;
            Update = update;
        }

        protected abstract Task Handle();
        public abstract StatesTypes StateTypesId { get; }

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

        protected virtual async Task HandleError()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, "Ввод не соответвует ожиданию");
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
        public static State GetState(StatesTypes stateTypesId, TelegramBotService botService, Update update)
        {
            switch (stateTypesId)
            {
                case StatesTypes.InitialState:
                    return new InitialState(botService, update);
                case StatesTypes.Exchange:
                    return new ExchangeState(botService, update);
                case StatesTypes.PayStart:
                    return new PaymentStartState(botService, update);
                default:
                    return GetDefaultState(botService, update);
            }
        }

        public static State GetDefaultState(TelegramBotService botService, Update update) => new InitialState(botService, update);
        #endregion
    }

    internal enum StatesTypes
    {
        InitialState = 0,
        Exchange,
        PayStart
    }
}
