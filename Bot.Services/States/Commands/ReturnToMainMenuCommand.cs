using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;

namespace Bot.Services.States.Commands
{
    internal class ReturnToMainMenuCommand:ICommand
    {
        public async Task Execute(TelegramBotService botService)
        {
            await botService.SetState(new InitialState(botService, null));
        }
    }
}
