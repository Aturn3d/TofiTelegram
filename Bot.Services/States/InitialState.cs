using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Bot.Services.States.Base
{
    internal class InitialState:State
    {
        internal const string commandsList = @"Список команд
/exchange - информация о курсе валют по отношению к BYN
/payment - cписок платежей
/help - список доступных команд
/return - отменить текущую операцию и вернуться в главное меню";

        internal InitialState(TelegramBotService botService, Update update) : base(botService, update) { }

        protected override async Task Handle()
        {
          await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, commandsList);
        }

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, commandsList);
        }

        public override StatesTypes StateTypesId => StatesTypes.InitialState;
        
    }
}
