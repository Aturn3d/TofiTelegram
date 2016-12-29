using System;
using System.Threading.Tasks;
using Bot.Services.States.Commands;
using Telegram.Bot.Types;

namespace Bot.Services.States.Base
{
    internal abstract class State
    {
        protected Update Update;
        protected TelegramBotService BotService;
        protected abstract Task Handle();
        public abstract StatesTypes StateTypesId { get; }

        public async Task HandleUpdate(TelegramBotService botService, Update update)
        {
            BotService = botService;
            Update = update;
            var handled = await HandleCommands();
            if (!handled) {
                await Handle();
            }
        }

        protected virtual Task HandleError()
        {
            throw new NotImplementedException();
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
    }
}
