using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;

namespace Bot.Services.States.Commands
{
    public class HelpCommand:ICommand
    {
        public async Task Execute(TelegramBotService botService)
        {
            await botService.Bot.SendTextMessageAsync(botService.User.ChatId, InitialState.commandsList);
        }
    }
}
