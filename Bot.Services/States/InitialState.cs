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
        private const string commandsList = @"Список команд
/exchange";
        protected override async Task Handle()
        {
          await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, commandsList);
        }

        public override StatesTypes StateTypesId => StatesTypes.InitialState;
    }
}
