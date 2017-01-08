using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;

namespace Bot.Services.States.Commands
{
    public class ExchangeCommand:ICommand
    {
      
        public Task Execute(TelegramBotService botService)
        {
            botService.SetState(new ExchangeState(botService, null));
            return State.TaskCompleted;
        }
    }
}
