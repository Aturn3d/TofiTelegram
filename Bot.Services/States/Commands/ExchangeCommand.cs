using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.States.Commands
{
    public class ExchangeCommand:ICommand
    {
        private const string text = @"Пожалуйста,введиде трехбуквенный код валюты в формате ИСО 4217 (например USD)";
        public async Task Execute(TelegramBotService botService)
        {
            await
                botService.Bot.SendTextMessageAsync(botService.User.ChatId,
                    text);
            botService.SetState(new ExchangeState());
        }
    }
}
