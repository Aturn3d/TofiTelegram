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
        internal const string commandsList = @"List of Commands:
/exchange - Information about the currencies exchange rate
/payment - List of payments
/lastPayments - 10 last payments
/help - A list of available commands
/return - Cancel the current operation and return to the main menu
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
