using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Services.States.Base;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.Commands
{
    internal class PaymentCommand:ICommand
    {
        public async Task Execute(TelegramBotService botService)
        {
            botService.SetState(new PaymentStartState(botService, null));
            await State.TaskCompleted;
        }
    }
}
