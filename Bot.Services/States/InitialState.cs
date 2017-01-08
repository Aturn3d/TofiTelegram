using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Services.States.Base
{
    internal class InitialState:State
    {
        internal const string commandsList = @"Список команд
/exchange - информация о курсе валют по отношению к BYN
/payment - cписок платежей
/lastPayments - 10 last payments
/help - список доступных команд
/return - отменить текущую операцию и вернуться в главное меню
Test card numbers: 
            4007000000027
            4012888818888
            4111111111111111
To Decline transaction use Cvv code 111";

        internal InitialState(TelegramBotService botService, Update update) : base(botService, update) { }

        protected override async Task Handle()
        {
            await TaskCompleted;
            // await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, commandsList, replyMarkup: new ReplyKeyboardHide());
        }

        public override async Task PrepareState()
        {
            await BotService.Bot.SendTextMessageAsync(BotService.User.ChatId, commandsList, replyMarkup: new ReplyKeyboardHide());
        }

        internal override StatesTypes StateTypesId => StatesTypes.InitialState;
        
    }
}
